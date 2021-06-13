using Lazy.StateManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public PlayerController controller;
    public Animator playerAnimator;
    Type lastState;

    private void Start()
    {
        controller.stateMachine.OnStateChange += OnStateChange;
    }

    private void OnStateChange(IState newState)
    {
        var stateType = newState.GetType();

        if(stateType == typeof(StandingState))
        {
            playerAnimator.Play("IdleAnim");
        }
        else if(stateType == typeof(WalkingState))
        {
            playerAnimator.Play("RunAnim");
        }
        else if (stateType == typeof(JumpingState))
        {
            playerAnimator.Play("JumpAnim");
        }
        else if(stateType == typeof(WallJumpState))
        {
            playerAnimator.Play("WallJumpAnim");
        }
        else if (stateType == typeof(WallClingState))
        {
            playerAnimator.Play("WallClingAnim");
        }
        else if(stateType == typeof(InAirState) && lastState != typeof(JumpingState) && lastState != typeof(WallJumpState))
        {
            playerAnimator.Play("InAirAnim");
        }

        lastState = stateType;
    }
}
