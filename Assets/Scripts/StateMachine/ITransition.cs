using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lazy.StateManagement
{
    public interface ITransition
    {
        IState To { get; set; }
        ICondition Condition { get; set; }
        void OnTransition();
    }
}
