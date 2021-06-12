using Lazy.StateManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerStates/WallJumpState")]
public class WallJumpState : SerializedScriptableObject, IState
{
    public float jumpHeight;
    public float pushFactor = 10;
    PlayerParameters parameters;

    public void Initialize(PlayerParameters parameters)
    {
        this.parameters = parameters; 
    }

    public void OnEnter()
    {
        parameters.gravity = Mathf.Sqrt(2f * -Physics2D.gravity.y * parameters.gravityMultiplier * jumpHeight);
        if (parameters.hitLeft)
        {
            parameters.isLookingRight = true;
            parameters.SetHorizontalForce(pushFactor);
        }
        if (parameters.hitRight)
        {
            parameters.isLookingRight = false;
            parameters.SetHorizontalForce(-pushFactor);
        }

        parameters.flipLocked = true;
    }

    public void OnExit()
    {
        parameters.flipLocked = false;
    }

    public void OnUpdate(float delta)
    {
        
    }
}
