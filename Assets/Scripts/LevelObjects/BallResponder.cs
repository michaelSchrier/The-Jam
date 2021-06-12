using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallResponder : MonoBehaviour
{
    public UnityEvent onBallEvent;
    public bool destroyBall = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (destroyBall)
                Destroy(collision.gameObject);
            
            onBallEvent.Invoke();
        }
    }
}
