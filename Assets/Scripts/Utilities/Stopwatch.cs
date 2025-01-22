using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Stopwatch : MonoBehaviour
{
    private DateTime startTime;
    private TextMeshPro stopwatchText;
    [Tooltip("Called when the stopwatch is stopped. Passes the value of the stopwatch as a parameter")]
    public UnityEvent<double> onStopwatchStopped;
    // Start is called before the first frame update
    void Start()
    {
        StopStopwatch();
    }

    public void StartStopwatch()
    {
        gameObject.SetActive(true);
        stopwatchText = GetComponent<TextMeshPro>();
        startTime = DateTime.Now;
        StartCoroutine(TimerLoop());
    }

    public void StopStopwatch()
    {
        StopAllCoroutines();
        onStopwatchStopped.Invoke((DateTime.Now - startTime).TotalSeconds);
        gameObject.SetActive(false);
    }

    private IEnumerator TimerLoop()
    {
        while (true)
        {
            TimeSpan timerValue = DateTime.Now - startTime;
            stopwatchText.text = string.Format("{0:00}:{1:00}:{2:000}", timerValue.Minutes, timerValue.Seconds, timerValue.Milliseconds);
            yield return null;
        }
    }
}
