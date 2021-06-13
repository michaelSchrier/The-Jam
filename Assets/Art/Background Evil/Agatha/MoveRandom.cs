using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandom : MonoBehaviour
{
    Vector2 startPos;
    float offset;
    public AnimationCurve curve;

    void Start()
    {
        offset = Random.Range(0f, 1f);
        startPos = transform.localPosition;
    }


    void Update()
    {
        transform.localPosition = new Vector2(startPos.x + curve.Evaluate(Mathf.PingPong(Time.time + offset, 1)), startPos.y);
    }
}
