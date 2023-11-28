using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeShowerScenarioManager : MonoBehaviour
{

    public GameObject levelClearedObject;
    private bool liquidBursted = false;
    public ParticleSystem particleSystemToActivate;
    public GameObject burstPopUp;


    public void Start()
    {
        if (particleSystemToActivate != null)
        {
            particleSystemToActivate.Stop();
        }
    }
    public void Aim() {

        if (liquidBursted)
        {
            levelClearedObject.SetActive(true);
        }
        
    }

    public void BurstLiquid()
    {
        if (liquidBursted)
        {
            return;
        }
        liquidBursted = true;
        if (particleSystemToActivate != null)
        {
            particleSystemToActivate.Play();
        }

        burstPopUp.SetActive(true);
    }
}

