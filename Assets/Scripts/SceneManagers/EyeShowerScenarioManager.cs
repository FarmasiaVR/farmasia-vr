using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EyeShowerScenarioManager : MonoBehaviour
{

    private bool liquidBursted = false;

    public UnityEvent LiquidBursts;
    public UnityEvent MissionComplete;

    public void Aim() {

        if (liquidBursted)
        {
            MissionComplete.Invoke();
        }
        
    }

    public void BurstLiquid()
    {
        if (liquidBursted)
        {
            return;
        }
        liquidBursted = true;

        LiquidBursts.Invoke();
    }
}

