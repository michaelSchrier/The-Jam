using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using System;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin perlin;
    public CinemachineConfiner confiner;
    float currentAmp;
    float currentFreq;
    float shakeDuration;
    public AnimationCurve shakeCurve;
    float counter;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        confiner = GetComponent<CinemachineConfiner>();

        Hub.Default.Subscribe<CameraShakeMessage>(this, CameraShakeMessageHandler);
        Hub.Default.Subscribe<SetCameraFocusMessage>(this, SetCameraFocusHandler);
        Hub.Default.Subscribe<SetConfinerMessage>(this, SetConfinerHandler);
    }

    private void SetConfinerHandler(SetConfinerMessage obj)
    {
        confiner.m_BoundingShape2D = obj.Collider;
        confiner.InvalidatePathCache();
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<CameraShakeMessage>(this, CameraShakeMessageHandler);
        Hub.Default.Unsubscribe<SetCameraFocusMessage>(this, SetCameraFocusHandler);
        Hub.Default.Unsubscribe<SetConfinerMessage>(this, SetConfinerHandler);
    }

    void SetCameraFocusHandler(SetCameraFocusMessage message)
    {
        cam.Follow = message.Target;
    }

    void CameraShakeMessageHandler(CameraShakeMessage message)
    {
        ShakeCam(message.amplitude, message.frequency, message.duration);
    }

    [Button]
    public void ShakeCam(float amplitude, float frequency, float duration)
    {
        currentAmp = amplitude;
        currentFreq = frequency;
        shakeDuration = duration;
        counter = 0;
    }

    public void StopShake()
    {
        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;
    }

    void Update()
    {
        if (counter < shakeDuration)
        {
            perlin.m_AmplitudeGain = currentAmp * shakeCurve.Evaluate(counter / shakeDuration);
            perlin.m_FrequencyGain = currentFreq * shakeCurve.Evaluate(counter / shakeDuration);

            counter += Time.deltaTime;
        }
        else
        {
            StopShake();
        }
    }
}

public class CameraShakeMessage
{
    public float amplitude, frequency, duration;

    public CameraShakeMessage(float amplitude, float frequency, float duration)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.duration = duration;
    }
}

public class SetCameraFocusMessage
{
    public Transform Target { get; }

    public SetCameraFocusMessage(Transform target)
    {
        Target = target;
    }
}

public class SetConfinerMessage
{
    public PolygonCollider2D Collider { get; }

    public SetConfinerMessage(PolygonCollider2D collider)
    {
        Collider = collider;
    }
}
