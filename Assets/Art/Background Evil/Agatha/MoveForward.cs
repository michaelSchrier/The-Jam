using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public Vector2 moveDirection = Vector2.right;
    public bool followY = true;
    bool move;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (move)
        {
            transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

            if (followY)
            {
                transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(cam.transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }

    public void Move()
    {
        move = true;
    }
}
