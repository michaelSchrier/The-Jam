using Lazy.StateManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/ThrowingState")]
public class ThrowingState : SerializedScriptableObject, IState
{
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate(float delta)
    {
        throw new System.NotImplementedException();
    }
}

