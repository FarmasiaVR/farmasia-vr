using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class TimerButtonPCM : MonoBehaviour
{

    public ClockController timerManager; 
    public TMP_Text buttonText;
    public bool isStartButton = true; 
    private void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    public void OnButtonPressed(SelectEnterEventArgs args)
    {        
        toggleButton();
    }

    public void toggleButton(){

        if (isStartButton && timerManager.plate == 8){
            timerManager.StartTimer();
            buttonText.text = "Stop Waiting";
            isStartButton = false;
        }

        else{
            timerManager.StopTimer();
            buttonText.text = "Start Waiting";
            isStartButton = true;
        }

    }

}
