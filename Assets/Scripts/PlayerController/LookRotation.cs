using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour
{
    public PlayerController controller;
    Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.parameters.isLookingRight)
        {
            transform.localScale = originalScale;
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x * -1, originalScale.y, originalScale.z);
        }
    }
}
