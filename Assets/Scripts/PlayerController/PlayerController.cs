using Lazy.StateManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : SerializedMonoBehaviour
{
    public float groundCheckLength = 0.02f;
    public float skinWidth = 0.02f;
    public int numOfGroundChecks = 2;
    public PlayerParameters parameters;
    [ShowInInspector] StateMachine stateMachine = new StateMachine();

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

    Rigidbody2D rb2d;
    BoxCollider2D bc2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        parameters.isLookingRight = true;

        LoadStateInstances();
        SetUpTransitions();
        stateMachine.ChangeState(standingState);
    }

    void Update()
    {
        parameters.playerInput = CaptureInput();
    }

    private void FixedUpdate()
    {
        LookUpdate();
        SensoryUpdate();
        parameters.velocityThisFrame = Vector2.zero;
        UpdateGravity();

        stateMachine.Tick(Time.deltaTime);

        rb2d.velocity = CalculateFinalVelocity();
    }

    Vector2 CalculateFinalVelocity()
    {
        parameters.velocityThisFrame.y += parameters.gravity;

        var angle = Vector2.SignedAngle(parameters.groundNormal, Vector2.up);
        var final = Quaternion.Euler(0, 0, -angle) * parameters.playerInput * 6f;
        Debug.DrawRay(bc2d.bounds.center, parameters.groundNormal * 2, Color.blue);
        Debug.DrawRay(bc2d.bounds.center, final.normalized * 2, Color.blue);
        parameters.velocityThisFrame += (Vector2)final;
        return parameters.velocityThisFrame;
    }

    void LookUpdate()
    {
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

        jumpingState.Initialize(parameters);
    }

    Vector2 CaptureInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), 0);
    }

    void UpdateGravity()
    {
        parameters.gravity += Physics2D.gravity.y * parameters.gravityMultiplier * Time.deltaTime;

        if ((parameters.isGrounded && rb2d.velocity.y < 0.01f) || (parameters.hitTop && rb2d.velocity.y > -0.01f))
            parameters.gravity = 0;
    }

    void SensoryUpdate()
    {
        parameters.isGrounded = false;
        parameters.hitTop = false;
        parameters.groundNormal = Vector2.up;

        var leftOfBox = bc2d.bounds.center.x - bc2d.size.x / 2;
        var bottomOfBox = bc2d.bounds.center.y - bc2d.bounds.size.y / 2;
        var topOfBox = bc2d.bounds.center.y + bc2d.bounds.size.y / 2;
        var xSegmentLength = bc2d.bounds.size.x / (numOfGroundChecks - 1);
        var length = groundCheckLength + skinWidth;

        for (int i = 0; i < numOfGroundChecks; i++)
        {
            var origin = new Vector2(leftOfBox + xSegmentLength * i, bottomOfBox + skinWidth);
            var direction = Vector2.down;
            var hitData = Physics2D.Raycast(origin, direction, length, LayerMask.GetMask("Ground"));
            Debug.DrawRay(origin, direction * length, Color.red);

            if (hitData)
            {
                parameters.isGrounded = true;
                parameters.groundNormal = hitData.normal;
            }      
        }

        for (int i = 0; i < numOfGroundChecks; i++)
        {
            var origin = new Vector2(leftOfBox + xSegmentLength * i, topOfBox - skinWidth);
            var direction = Vector2.up;
            var hitData = Physics2D.Raycast(origin, direction, length, LayerMask.GetMask("Ground"));
            Debug.DrawRay(origin, direction * length, Color.red);

            if (hitData)
            {
                parameters.hitTop = true;
            }
        }
    }

    void SetUpTransitions()
    {
        stateMachine.OnStateChange += (IState state) => Debug.Log("Entered: " + state.GetType().Name);

        stateMachine.AddTransition(standingState, walkingState, () => parameters.playerInput.x != 0);
        stateMachine.AddTransition(walkingState, standingState, () => parameters.playerInput.x == 0);

        stateMachine.AddTransition(standingState, jumpingState, () => parameters.isGrounded && Input.GetKey(KeyCode.Space));
        stateMachine.AddTransition(walkingState, jumpingState, () => parameters.isGrounded && Input.GetKey(KeyCode.Space));
        stateMachine.AddTransition(jumpingState, inAirState, () => rb2d.velocity.y <= 0);
        stateMachine.AddTransition(jumpingState, inAirState, () => !Input.GetKey(KeyCode.Space) && stateMachine.TimeSinceStateChange > 0.05f, ()=> parameters.gravity = rb2d.velocity.y * 0.2f);

        stateMachine.AddTransition(inAirState, walkingState, () => parameters.isGrounded);
    }
}
