using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSingleton;

public class TimeManager : Singleton<TimeManager>
{
    private float currentTime = 0;
    public float CurrentTime => currentTime;
    // Start is called before the first frame update

    // Update is called once per frame
    public void ManualUpdate()
    {
        AddTime(Time.deltaTime);
    }

    public void AddTime(float time)
    {
        currentTime += time;
    }

    public void ResetTime()
    {
        currentTime = 0;
    }
}
