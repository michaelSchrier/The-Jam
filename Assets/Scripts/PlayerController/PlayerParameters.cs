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
    public bool isGrounded;
    public bool hitTop;
    public bool isLookingRight;
    public Vector2 groundNormal = Vector2.zero;
}
