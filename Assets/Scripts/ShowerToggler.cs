using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerToggler : MonoBehaviour
{
    public bool open;
    public AudioSource audioSource;
    public float loopStartTime;
    public float loopEndTime;
    public ParticleSystem water;
    private Coroutine turnOffShowerCoroutine;
    public ShowerExtinguisher showerExtinguisher;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            showerExtinguisher.Extinguish();
        }
    }

    // Initiates water VFX, terminates previous turnOffShower coroutine
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shower")
        {
            open = true;
            water.Play();
            
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                audioSource.time = 0;
            }


            Debug.Log("Shower should be on");
            
            // Stop the previous coroutine if it's still running
            if (turnOffShowerCoroutine != null)
            {
                StopCoroutine(turnOffShowerCoroutine);
                Debug.Log("Terminated previous coroutine");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shower")
        {
            if (audioSource.time >= loopEndTime)
            {
                audioSource.time = loopStartTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shower" && open)
        {

            audioSource.time = loopEndTime;
            audioSource.loop = false;

            turnOffShowerCoroutine =  StartCoroutine(turnOffShower());
        }
    }

    // Coroutine that turns off water vfx after 4 seconds have passed
    private IEnumerator turnOffShower()
    {
        Debug.Log("Shower should still be on for next 4 seconds");

        yield return new WaitForSeconds(4);

        open = false;

        water.Stop();
        Debug.Log("Shower should now be off");
        
    }


}
