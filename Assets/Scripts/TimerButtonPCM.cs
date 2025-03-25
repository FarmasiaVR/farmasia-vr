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

    private void OnButtonPressed(SelectEnterEventArgs args)
    {        
        if (timerManager.plate == 8)
        {
            if (isStartButton){
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

}
