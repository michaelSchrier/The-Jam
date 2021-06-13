using PubSub;
using UnityEngine;

public class TimeSlowManager : MonoBehaviour
{
    private float fixedTimeStep;

    void Awake()
    {
        Hub.Default.Subscribe<TimeSlowMessage>(this, SlowTimeMessageHandler);
        Hub.Default.Subscribe<ResetTimeSlowMessage>(this, ResetTimeMessageHandler);

        Time.fixedDeltaTime = 1 / 60f;
        fixedTimeStep = Time.fixedDeltaTime;
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<TimeSlowMessage>(this, SlowTimeMessageHandler);
        Hub.Default.Unsubscribe<ResetTimeSlowMessage>(this, ResetTimeMessageHandler);
    }

    void SlowTimeMessageHandler(TimeSlowMessage message)
    {
        SlowTime(message.amount);
    }

    void ResetTimeMessageHandler(ResetTimeSlowMessage message)
    {
        ResetSlow();
    }

    public void SlowTime(float amount)
    {
        Time.timeScale = amount;
        Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
    }

    public void ResetSlow()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
    }
}

public class TimeSlowMessage
{
    public float amount;

    public TimeSlowMessage(float amount)
    {
        this.amount = amount;
    }
}

public class ResetTimeSlowMessage { }
