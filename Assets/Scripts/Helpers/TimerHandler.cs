using System;
using System.Timers;
using UnityEngine;

public class TimerHandler
{
    private Timer timer;
    public Action callback;
    public float timerInSeconds = 5f;
    public bool IsTimerRunning { get; private set; }
    public bool IsTimerDone { get; private set; }
    private DateTime startTime;
    private DateTime endTime;

    /// <summary>
    /// TimerHandler class is used to create a timer that can be used in any class.
    /// It uses System.Timers.Timer to create a timer that can be used to call a function after a certain amount of time.
    /// Note: This timer happens outside of the Unity's main thread, so if it is to handle any visual elements, it should be done in the main thread and should be used sparingly.
    /// Example: You can use the bool IsTimerDone to check if the timer is done and then call the function that handles the visual elements in the Update function.
    /// </summary>

    // Constructor
    public TimerHandler(float timerInSeconds, Action action)
    {
        this.timerInSeconds = timerInSeconds;
        callback = action;
    }

    public void InitTimer()
    {
        Debug.Log("Timer is started");
        IsTimerRunning = true;
        IsTimerDone = false;
        SetDateTime();

        timer = new Timer(timerInSeconds * 1000);
        timer.Start();
        timer.Elapsed += (sender, e) => TimerCallback();
    }

    public void StopTimer()
    {
        IsTimerRunning = false;
        timer.Dispose();
        Debug.Log("Timer is stopped");
    }

    private void SetDateTime()
    {
        startTime = DateTime.Now;
        endTime = startTime.AddSeconds(timerInSeconds);
    }

    private void TimerCallback()
    {
        Debug.Log("Timer is done");
        StopTimer();
        IsTimerDone = true;
    }

    public float GetTimeLeft()
    {
        if (!IsTimerRunning)
        {
            return 0;
        }
        TimeSpan timeLeft = endTime - DateTime.Now;
        return (float)timeLeft.TotalSeconds;
    }
}

