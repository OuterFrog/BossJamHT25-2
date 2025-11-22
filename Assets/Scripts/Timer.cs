using UnityEngine;

public class ClockTimer
{
    public float maxTime = 0;
    public float currentTime = 0;
    public bool timeIsUp = false;

    public ClockTimer(float newMaxTime)
    {
        maxTime = newMaxTime;
        currentTime = maxTime;
    }

    public void Tick(float deltaTime)
    {
        currentTime -= deltaTime;

        if(currentTime <= 0)
        {
            timeIsUp = true;
        }
    }

    public void Reset()
    {
        timeIsUp = false;
        currentTime = maxTime;
    }
}
