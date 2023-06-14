using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private DateTime startTime;
    private TextMeshPro timerText;
    // Start is called before the first frame update
    void Start()
    {
        StopTimer();
    }

    public void StartTimer()
    {
        gameObject.SetActive(true);
        timerText = GetComponent<TextMeshPro>();
        startTime = DateTime.Now;
        StartCoroutine(TimerLoop());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private IEnumerator TimerLoop()
    {
        while (true)
        {
            TimeSpan timerValue = DateTime.Now - startTime;
            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", timerValue.Minutes, timerValue.Seconds, timerValue.Milliseconds);
            yield return null;
        }
    }
}
