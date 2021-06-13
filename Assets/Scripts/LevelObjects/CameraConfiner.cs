using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    private void Start()
    {
        var collider = GetComponent<PolygonCollider2D>();
        Hub.Default.Publish(new SetConfinerMessage(collider));
    }
}
