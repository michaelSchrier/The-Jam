using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin perlin;
    float currentAmp;
    float currentFreq;
    float shakeDuration;
    public AnimationCurve shakeCurve;
    float counter;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Hub.Default.Subscribe<CameraShakeMessage>(this, CameraShakeMessageHandler);
        Hub.Default.Subscribe<SetCameraFocusMessage>(this, SetCameraFocusHandler);
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<CameraShakeMessage>(this, CameraShakeMessageHandler);
        Hub.Default.Unsubscribe<SetCameraFocusMessage>(this, SetCameraFocusHandler);
    }

    void SetCameraFocusHandler(SetCameraFocusMessage message)
    {
        Debug.Log("Cam Set");
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
