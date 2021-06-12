using Lazy.StateManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/JumpingState")]
public class JumpingState : SerializedScriptableObject, IState
{
    public float jumpHeight;
    PlayerParameters parameters;

    public void Initialize(PlayerParameters controller)
    {
        this.parameters = controller;
    }

    public void OnEnter()
    {
        parameters.gravity = Mathf.Sqrt(2f * -Physics2D.gravity.y * parameters.gravityMultiplier * jumpHeight);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate(float delta)
    {
        
    }
}
