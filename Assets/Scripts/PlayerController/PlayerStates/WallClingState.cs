using Lazy.StateManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerStates/WallClingState")]
public class WallClingState : SerializedScriptableObject, IState
{
    public float slideSpeed;
    float originalTerminal;
    PlayerParameters parameters;

    public void Initialize(PlayerParameters parameters)
    {
        this.parameters = parameters; 
    }

    public void OnEnter()
    {
        originalTerminal = parameters.terminalVelocity;
        parameters.terminalVelocity = slideSpeed;

        parameters.flipLocked = true;
        if (parameters.hitLeft)
            parameters.isLookingRight = true;
        if (parameters.hitRight)
            parameters.isLookingRight = false;
    }

    public void OnExit()
    {
        parameters.terminalVelocity = originalTerminal;
        parameters.flipLocked = false;
    }

    public void OnUpdate(float delta)
    {
        
    }
}
