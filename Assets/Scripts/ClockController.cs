using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockController : MonoBehaviour
{
    [SerializeField]
    public TMP_Text timerText;
    private float elapsedTime;
    private bool start;
    public int plate = 8;
    public const float timeMultiplier = 60f;

    private bool ventingComplete = false;
  
    void Start()
    {
        
    }

    void Update()
    {   
        if (start){
            elapsedTime += Time.deltaTime * timeMultiplier;
            UpdateTimerText();
        }

    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = $"{minutes:D2} min";
        if (minutes > 14 && !ventingComplete){
            completeVenting();
        }
    }

    public void ventingEvent(bool isVenting){
        if(isVenting){
            plate++;
        }
        else{
            plate--;
        }
    }

    public void StartTimer(){
        if (plate == 8){
            start = true;
        }
    }

    public void StopTimer(){
        start = false;
    }

    private void completeVenting(){
        ventingComplete = true;
        //fire event for PCM to complete the task
        Logger.Print("Hep Hep Hep");
    }
}
