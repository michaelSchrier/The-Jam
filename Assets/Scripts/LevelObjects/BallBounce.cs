using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    public GameObject bounceDirectionIndicator;
    public float bounceSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            var rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            rigidBody.velocity = (bounceDirectionIndicator.transform.position - transform.position).normalized * bounceSpeed;
        }
    }
}
