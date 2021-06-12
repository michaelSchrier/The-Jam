using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowManager : MonoBehaviour
{
    private float fixedTimeStep;

    public static TimeSlowManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.fixedDeltaTime = 1 / 60f;
        fixedTimeStep = Time.fixedDeltaTime;
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
