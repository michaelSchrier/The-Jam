using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class PlayerParameters
{
    public Vector2 velocityThisFrame;
    public Vector2 playerInput;
    public float gravity;
    public float gravityMultiplier = 1;
    public float lastHorizontalForce = 1;
    public float currentHorizontalForce;
    public float horizontalForceReducer = 10f;
    public float terminalVelocity = -20f;
    public bool isGrounded;
    public bool hitTop, hitRight, hitLeft;
    public bool isLookingRight;
    public bool flipLocked;
    public Vector2 groundNormal = Vector2.zero;

    public void SetHorizontalForce(float value)
    {
        lastHorizontalForce = value;
        currentHorizontalForce = value;
    }
}
