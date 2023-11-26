using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FireHoseSystemBehaviour : MonoBehaviour
{
    public GameObject thisObject;
    public Rigidbody thisRigidbody;
    public ParticleSystem particleSystemToActivate;
    public AudioSource audioToActivate;
    public GameObject lever;
    public GameObject leverToCheck;
    bool hoseEndIsOn = false;
    bool leverIsOn = false;
    public FireExtinguisher extinguisherScript;
    // Start is called before the first frame update
    void Start()
    {
        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = false;
        }

        if (particleSystemToActivate != null)
        {
            particleSystemToActivate.Stop();
            audioToActivate.Stop();
            hoseEndIsOn = false;
        }
        if (leverToCheck != null) {
        if (Mathf.RoundToInt((leverToCheck.transform.eulerAngles.z)) == 90) {
            leverIsOn = true;
        }
        }
    }

    public void TakenToHand() {
        //Debug.Log("TakenToHand method called");
        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = true;
        }
    }

    public void TurnOnTheNozzle()
    {
        //starting particlesystem if hose is not on and lever is on, otherwise shutting it down
        //Debug.Log("TurnOnTheNozzle method called");
        if (particleSystemToActivate != null && !hoseEndIsOn && leverIsOn && audioToActivate != null)
        {
            particleSystemToActivate.Play();
            audioToActivate.Play();
            
            hoseEndIsOn = true;
            if (extinguisherScript != null) {
                extinguisherScript.Extinguish();
            }
        }
        else if (particleSystemToActivate != null && !hoseEndIsOn && !leverIsOn && audioToActivate != null)
        {
            hoseEndIsOn = true;
        }
        else if (particleSystemToActivate != null && hoseEndIsOn && audioToActivate != null)
        {
            hoseEndIsOn = false;
            particleSystemToActivate.Stop();
            audioToActivate.Stop();
            if (extinguisherScript != null)
            {
                extinguisherScript.StopExtinguish();
            }
        }
    }

    public void LeverIsTurned()
    {
        //Debug.Log("LeverIsTurned method called");

        if (particleSystemToActivate != null && audioToActivate != null) {
            if (leverIsOn && hoseEndIsOn)
            { 
                leverIsOn = false;
                particleSystemToActivate.Stop();
                audioToActivate.Stop();
                if (extinguisherScript != null)
                {
                    extinguisherScript.StopExtinguish();
                }
            }
            else if (leverIsOn && !hoseEndIsOn)
            {
                leverIsOn = false;
                particleSystemToActivate.Stop();
                audioToActivate.Stop();
                if (extinguisherScript != null)
                {
                    extinguisherScript.StopExtinguish();
                }
            }
            else if (!leverIsOn && hoseEndIsOn)
            {
                leverIsOn = true;
                particleSystemToActivate.Play();
                audioToActivate.Play();
                if (extinguisherScript != null)
                {
                    extinguisherScript.Extinguish();
                }
            }
            else if (!leverIsOn && !hoseEndIsOn)
            {
                leverIsOn = true;
                particleSystemToActivate.Stop();
                audioToActivate.Stop();
                if (extinguisherScript != null)
                {
                    extinguisherScript.StopExtinguish();
                }
            }
        }
    }
}
