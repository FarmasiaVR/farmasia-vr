using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
//This script is responsible for the working of the head of a firehose and its possible states while having a lever.
//The hose can shoot water if its on and the lever is on. Otherwise it can not. This pretty much holds for real life and this game.
//There are some currently unnecessary parts with when the working of the hose was a bit different.

public class FireHoseSystemBehaviour : MonoBehaviour
{
    //reference to hosehead itself
    public GameObject thisObject;
    //hoseheads own rigidbody
    public Rigidbody thisRigidbody;
    //hoseheads own system
    public ParticleSystem particleSystemToActivate;
    //hoseheads own system
    public AudioSource audioToActivate;
    //lever object
    public GameObject lever;
    //lever object to check for rotation (is on or off). Most likely same as earlier lever
    public GameObject leverToCheck;
    bool hoseEndIsOn = false;
    bool leverIsOn = false;
    //extinguisher script for older fire model, hoseheads own script
    public FireExtinguisher extinguisherScript;
    //extinguisher script for newer spreadable fire model, hoseheads own script
    public ExtingushSpreadableFire spreadFireExtScript;

    void Start()
    {
        //adjust starting parameters well fit for an untouched system, though if lever is already turned on it registers.
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
        //Was used earlier when hoseheads did not start with gravity.
        //Debug.Log("TakenToHand method called");
        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = true;
        }
    }

    public void TurnOnTheNozzle()
    //This method if responsible for different states of the hose when turning it on. The particle and audio systems and extinguishing should only work
    //when the hosehead is on and the lever is on.
    {
        //Debug.Log("TurnOnTheNozzle method called");
        if (particleSystemToActivate != null && !hoseEndIsOn && leverIsOn && audioToActivate != null)
        {
            particleSystemToActivate.Play();
            audioToActivate.Play();
            
            hoseEndIsOn = true;
            if (extinguisherScript != null) {
                extinguisherScript.Extinguish();
            }
            if (spreadFireExtScript != null)
            {
                spreadFireExtScript.Extinguish();
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
            if (spreadFireExtScript != null)
            {
                spreadFireExtScript.StopExtinguishing();
            }
        }
    }

    public void LeverIsTurned()
    //This method is responsible for whats happening when lever is turned, if lever is on it enables hosehead to do its functions.
    //If the lever is turned while hosehead is functioning it should turn off the particle and audio systems and stop extinguishing.
    //However it should not change the fact the hosehead is still on, so when the lever is turned
    //again the actions may continue without further interaction with the hosehead.
    //There are many statements and possible changes should be made carefully.
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
                if (spreadFireExtScript != null)
                {
                    spreadFireExtScript.StopExtinguishing();
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
                if (spreadFireExtScript != null)
                {
                    spreadFireExtScript.StopExtinguishing();
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
                if (spreadFireExtScript != null)
                {
                    spreadFireExtScript.Extinguish();
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
                if (spreadFireExtScript != null)
                {
                    spreadFireExtScript.StopExtinguishing();
                }
            }
        }
    }
}
