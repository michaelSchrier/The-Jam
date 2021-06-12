using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject targetingReticule;
    public float firePower = 10;
    public float timeSlowIntensity = 0.2f;

    public GameObject ballPrefab;
    private GameObject spawnedBall;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        targetingReticule.SetActive(false);
    }

    private void Update()
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
        Vector2 direction = Input.mousePosition - screenPoint;
        direction.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
            Hub.Default.Publish(new TimeSlowMessage(timeSlowIntensity));
            targetingReticule.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            FireBall(direction * firePower);
            Hub.Default.Publish(new ResetTimeSlowMessage());
            targetingReticule.SetActive(false);
        }

        transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(direction, Vector2.up));
    }

    void FireBall(Vector2 fireDirection)
    {
        spawnedBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        var ballScript = spawnedBall.GetComponent<TheBall>();
        ballScript.creator = gameObject;
        ballScript.Launch(fireDirection);
    }
}
