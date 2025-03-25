using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TimerButtonPCM : MonoBehaviour
{

    public ClockController timerManager; 
    public bool isStartButton = true; 
    private void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (timerManager != null)
        {
            if (isStartButton)
                timerManager.StartTimer();
            else
                timerManager.StopTimer();
        }
    }

}
