using Lazy.StateManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerStates/WalkingState")]
public class WalkingState : SerializedScriptableObject, IState
{
    public float maxSpeed;

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
