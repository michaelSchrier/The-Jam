using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class MoveAlongPath : MonoBehaviour
{
    public PathCreator pathCreator;
    public float travelSpeed;

    public void MoveZip()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float position = 0;
        while(position < pathCreator.path.length)
        {
            position += Time.deltaTime * travelSpeed;
            transform.position = pathCreator.path.GetPointAtDistance(position, EndOfPathInstruction.Stop);
            yield return null;
        }
    }
}
