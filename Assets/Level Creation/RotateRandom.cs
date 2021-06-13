using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandom : MonoBehaviour
{
    public float rotateSpeed = 10f;

    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 180));
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + (rotateSpeed * Time.deltaTime));
    }
}
