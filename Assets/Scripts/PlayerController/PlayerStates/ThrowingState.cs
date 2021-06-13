using Lazy.StateManagement;
using PubSub;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/ThrowingState")]
public class ThrowingState : SerializedScriptableObject, IState
{
    PlayerController controller;
    public GameObject reticulePrefab;
    GameObject targetingReticule;
    public float firePower = 10;
    public float timeSlowIntensity = 0.2f;
    Camera cam;
    public GameObject ballPrefab;
    private GameObject spawnedBall;
    public Vector2 direction;

    public void Initialize(PlayerController controller)
    {
        this.controller = controller;
        cam = Camera.main;
    }

    public void OnEnter()
    {
        Hub.Default.Publish(new TimeSlowMessage(timeSlowIntensity));
        targetingReticule = Instantiate(reticulePrefab, controller.transform.position, Quaternion.identity);
        targetingReticule.transform.SetParent(controller.transform);
    }

    public void OnExit()
    {
        Hub.Default.Publish(new ResetTimeSlowMessage());
        FireBall(direction * firePower);
        Destroy(targetingReticule);
    }

    public void OnUpdate(float delta)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(targetingReticule.transform.position);
        direction = Input.mousePosition - screenPoint;
        direction.Normalize();

        targetingReticule.transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(direction, Vector2.up));
    }

    void FireBall(Vector2 fireDirection)
    {
        spawnedBall = Instantiate(ballPrefab, controller.transform.position, Quaternion.identity);
        var ballScript = spawnedBall.GetComponent<TheBall>();
        ballScript.creator = controller.gameObject;
        ballScript.Launch(fireDirection);
    }
}

