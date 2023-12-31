using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{
    private float time = 0;
    private TextMeshProUGUI timerText;

    public UnityEvent OnTimerEnd;
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerPause;

    public int maxTime = 600;
    
    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Ten minutes
        if (time < maxTime)
        {
            time += Time.deltaTime;
        }
        else
        {
            TimerEnd();
        }

        DisplayTime(time);
    }

    public float GetTime()
    {
        return time;
    }

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TimerEnd()
    {
        time = maxTime;
        StopAllCoroutines();
        Time.timeScale = 0;
        OnTimerEnd.Invoke();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        OnTimerPause.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        OnTimerStart.Invoke();
    }
}
