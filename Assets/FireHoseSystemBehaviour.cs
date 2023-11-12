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
    bool hoseEndIsOn;
    bool leverIsOn;
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
        leverIsOn = false;
        hoseEndIsOn = false;
    }

    public void TakenToHand() { 
        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = true;
            //Debug.Log("TakenToHand method called");
        }
    }

    public void TurnOnTheNozzle()
    {
        //starting particlesystem if hose is not on and lever is on, otherwise shutting it down
        if (particleSystemToActivate != null && !hoseEndIsOn && leverIsOn && audioToActivate != null)
        {
            particleSystemToActivate.Play();
            audioToActivate.Play();
            
            hoseEndIsOn = true;
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
        }
    }

    public void LeverIsTurned()
    {
        if (particleSystemToActivate != null && audioToActivate != null) {
            if (leverIsOn && hoseEndIsOn)
            { 
                leverIsOn = false;
                particleSystemToActivate.Stop();
                audioToActivate.Stop();
            }
            else if (leverIsOn && !hoseEndIsOn)
            {
                leverIsOn = false;
                particleSystemToActivate.Stop();
                audioToActivate.Stop();
            }
            else if (!leverIsOn && hoseEndIsOn)
            {
                leverIsOn = true;
                particleSystemToActivate.Play();
                audioToActivate.Play();
            }
            else if (!leverIsOn && !hoseEndIsOn)
            {
                leverIsOn = true;
                particleSystemToActivate.Stop();
                audioToActivate.Stop();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
