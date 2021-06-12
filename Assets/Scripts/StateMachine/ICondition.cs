using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lazy.StateManagement
{
    public interface ICondition
    {
        bool Validate();
    }
}
