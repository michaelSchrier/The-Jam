using Lazy.StateManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/StandingState")]
public class StandingState : SerializedScriptableObject, IState
{
    public int test = 1;

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        
    }

    public void OnUpdate(float delta)
    {
        
    }
}
