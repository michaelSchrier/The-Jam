using Lazy.StateManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PubSub;

public class PlayerController : SerializedMonoBehaviour
{
    public float horizontalCheckLength = 0.02f;
    public float verticalCheckLength = 0.04f;
    public float skinWidth = 0.02f;
    public int numOfChecks = 2;
    public float playerSpeed = 7.5f;
    bool didJumpThisFrame = false;
    bool didLeftClickThisFrame = false;
    public PlayerParameters parameters;
    public static PlayerController intance;
    public StateMachine stateMachine = new StateMachine();

    [TabGroup("Standing")] [InlineEditor] 
    public StandingState standingState;

    [TabGroup("Walking")] [InlineEditor] 
    public WalkingState walkingState;

    [TabGroup("InAir")] [InlineEditor] 
    public InAirState inAirState;

    [TabGroup("Jumping")] [InlineEditor] 
    public JumpingState jumpingState;

    [TabGroup("Throwing")] [InlineEditor] 
    public ThrowingState throwingState;

    [TabGroup("WallCling")]
    [InlineEditor]
    public WallClingState wallClingState;

    [TabGroup("WallJump")]
    [InlineEditor]
    public WallJumpState wallJumpState;

    public GameObject deathParticles;

    Rigidbody2D rb2d;
    BoxCollider2D bc2d;

    private void Awake()
    {
        intance = this;
        Hub.Default.Subscribe<PlayerDeathMessage>(this, KillPlayer);
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        parameters.isLookingRight = true;

        LoadStateInstances();
        SetUpTransitions();
        stateMachine.ChangeState(standingState);
    }

    private void Start()
    {
        Hub.Default.Publish(new SetCameraFocusMessage(transform));
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<PlayerDeathMessage>(this, KillPlayer);
    }



    public void KillPlayer(PlayerDeathMessage death)
    {
        gameObject.SetActive(false);
        Hub.Default.Publish(new ResetTimeSlowMessage());
        if (deathParticles)
            Instantiate(deathParticles, transform.position, Quaternion.identity);    
    }

    void Update()
    {
        parameters.playerInput = CaptureMovementInput();

        if (Input.GetKeyDown(KeyCode.Space))
            didJumpThisFrame = true;
        if (Input.GetMouseButtonDown(0))
            didLeftClickThisFrame = true;
    }

    private void FixedUpdate()
    {
        LookUpdate();
        SensoryUpdate();
        parameters.velocityThisFrame = Vector2.zero;
        UpdateGravity();
        UpdateHorizontalForce();

        stateMachine.Tick(Time.deltaTime);

        rb2d.velocity = CalculateFinalVelocity();

        didJumpThisFrame = false;
        didLeftClickThisFrame = false;
    }

    Vector2 CalculateFinalVelocity()
    {
        parameters.velocityThisFrame.y += parameters.gravity;

        var currentPlayer = parameters.playerInput.x * playerSpeed * (1 - (parameters.currentHorizontalForce / parameters.lastHorizontalForce));

        var final = new Vector2(currentPlayer + parameters.currentHorizontalForce, 0);

        parameters.velocityThisFrame += (Vector2)final;
        return parameters.velocityThisFrame;
    }

    void UpdateHorizontalForce()
    {
        parameters.currentHorizontalForce = Mathf.MoveTowards(parameters.currentHorizontalForce, 0, parameters.horizontalForceReducer * Time.deltaTime);
    }

    void LookUpdate()
    {
        if (parameters.flipLocked)
            return;

        if(parameters.playerInput.x > 0)
        {
            parameters.isLookingRight = true;
        }
        else if(parameters.playerInput.x < 0)
        {
            parameters.isLookingRight = false;
        }
    }

    void LoadStateInstances()
    {
        standingState = Instantiate(standingState);
        walkingState = Instantiate(walkingState);
        inAirState = Instantiate(inAirState);
        jumpingState = Instantiate(jumpingState);
        throwingState = Instantiate(throwingState);
        wallClingState = Instantiate(wallClingState);
        wallJumpState = Instantiate(wallJumpState);

        jumpingState.Initialize(parameters);
        wallClingState.Initialize(parameters);
        wallJumpState.Initialize(parameters);
        throwingState.Initialize(this);
    }

    Vector2 CaptureMovementInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), 0);
    }

    void UpdateGravity()
    {
        parameters.gravity += Physics2D.gravity.y * parameters.gravityMultiplier * Time.deltaTime;
        parameters.gravity = Mathf.Clamp(parameters.gravity, parameters.terminalVelocity, int.MaxValue);

        if ((parameters.isGrounded && rb2d.velocity.y < 0.01f) || (parameters.hitTop && rb2d.velocity.y > -0.01f))
            parameters.gravity = 0;
    }

    void SensoryUpdate()
    {
        parameters.isGrounded = false;
        parameters.hitTop = false;
        parameters.hitLeft = false;
        parameters.hitRight = false;
        parameters.groundNormal = Vector2.up;

        var leftOfBox = bc2d.bounds.center.x - bc2d.size.x / 2;
        var rightOfBox = bc2d.bounds.center.x + bc2d.size.x / 2;
        var bottomOfBox = bc2d.bounds.center.y - bc2d.bounds.size.y / 2;
        var topOfBox = bc2d.bounds.center.y + bc2d.bounds.size.y / 2;
        var xSegmentLength = bc2d.bounds.size.x / (numOfChecks - 1);
        var ySegmentLength = bc2d.bounds.size.y / (numOfChecks - 1);
        var horizontalLength = horizontalCheckLength + skinWidth;
        var verticalLength = verticalCheckLength + skinWidth;

        for (int i = 0; i < numOfChecks; i++)
        {
            var origin = new Vector2(leftOfBox + xSegmentLength * i, bottomOfBox + skinWidth);
            var direction = Vector2.down;
            var hitData = Physics2D.Raycast(origin, direction, verticalLength, LayerMask.GetMask("Ground"));
            Debug.DrawRay(origin, direction * verticalLength, Color.red);

            if (hitData)
            {
                parameters.isGrounded = true;
                parameters.groundNormal = hitData.normal;
            }      
        }

        for (int i = 0; i < numOfChecks; i++)
        {
            var origin = new Vector2(leftOfBox + xSegmentLength * i, topOfBox - skinWidth);
            var direction = Vector2.up;
            var hitData = Physics2D.Raycast(origin, direction, verticalLength, LayerMask.GetMask("Ground"));
            Debug.DrawRay(origin, direction * verticalLength, Color.red);

            if (hitData)
            {
                parameters.hitTop = true;
            }
        }

        for (int i = 0; i < numOfChecks; i++)
        {
            var origin = new Vector2(leftOfBox + skinWidth, bottomOfBox + ySegmentLength * i);
            var direction = Vector2.left;
            var hitData = Physics2D.Raycast(origin, direction, horizontalLength, LayerMask.GetMask("Ground"));
            Debug.DrawRay(origin, direction * horizontalLength, Color.red);

            if (hitData)
            {
                parameters.hitLeft = true;
            }
        }

        for (int i = 0; i < numOfChecks; i++)
        {
            var origin = new Vector2(rightOfBox - skinWidth, bottomOfBox + ySegmentLength * i);
            var direction = Vector2.right;
            var hitData = Physics2D.Raycast(origin, direction, horizontalLength, LayerMask.GetMask("Ground"));
            Debug.DrawRay(origin, direction * horizontalLength, Color.red);

            if (hitData)
            {
                parameters.hitRight = true;
            }
        }
    }

    void SetUpTransitions()
    {
        stateMachine.OnStateChange += (IState state) => Debug.Log("Entered: " + state.GetType().Name);

        stateMachine.AddTransition(standingState, walkingState, () => parameters.playerInput.x != 0);
        stateMachine.AddTransition(walkingState, standingState, () => parameters.playerInput.x == 0);

        stateMachine.AddTransition(standingState, inAirState, () => !parameters.isGrounded);
        stateMachine.AddTransition(walkingState, inAirState, () => !parameters.isGrounded);

        stateMachine.AddTransition(standingState, jumpingState, () => parameters.isGrounded && didJumpThisFrame);
        stateMachine.AddTransition(walkingState, jumpingState, () => parameters.isGrounded && didJumpThisFrame);
        stateMachine.AddTransition(jumpingState, inAirState, () => rb2d.velocity.y <= 0);
        stateMachine.AddTransition(jumpingState, inAirState, () => !Input.GetKey(KeyCode.Space) && stateMachine.TimeSinceStateChange > 0.05f, ()=> parameters.gravity = rb2d.velocity.y * 0.2f);

        stateMachine.AddTransition(inAirState, walkingState, () => parameters.isGrounded);


        bool HitSide()
        {
            return (parameters.hitRight || parameters.hitLeft);
        }


        stateMachine.AddTransition(inAirState, wallClingState, () => HitSide() && rb2d.velocity.y < 0);
        stateMachine.AddTransition(wallClingState, inAirState, () => !HitSide());
        stateMachine.AddTransition(wallClingState, standingState, () => parameters.isGrounded);

        stateMachine.AddTransition(wallClingState, wallJumpState, () => didJumpThisFrame);
        stateMachine.AddTransition(wallJumpState, inAirState, () => rb2d.velocity.y <= 0 || stateMachine.TimeSinceStateChange > 0.2f);

        stateMachine.AddTransition(inAirState, wallJumpState, () => HitSide() && didJumpThisFrame);

        stateMachine.AddAnyTransition(throwingState, () => didLeftClickThisFrame && throwingState.CanThrow);
        stateMachine.AddTransition(throwingState, standingState, () => !Input.GetMouseButton(0) || stateMachine.TimeSinceStateChange > 0.3f);//So short because time slow.
    }
}
