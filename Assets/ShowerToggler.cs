using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerToggler : MonoBehaviour
{
    public bool open;
    public ParticleSystem water;
    private Coroutine turnOffShowerCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initiates water VFX, terminates previous turnOffShower coroutine
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shower")
        {
            open = true;
            water.Play();
            Debug.Log("Shower should be on");
            
            // Stop the previous coroutine if it's still running
            if (turnOffShowerCoroutine != null)
            {
                StopCoroutine(turnOffShowerCoroutine);
                Debug.Log("Terminated previous coroutine");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shower" && open)
        {
            turnOffShowerCoroutine =  StartCoroutine(turnOffShower());
        }
    }

    // Coroutine that turns off water vfx after 4 seconds have passed
    private IEnumerator turnOffShower()
    {
        open = false;
        Debug.Log("Shower should still be on for next 4 seconds");

        yield return new WaitForSeconds(4);

        if (!open)
        {
            water.Stop();
            Debug.Log("Shower should now be off");
        }
        else
        {
            Debug.Log("Cancelled turnoff coroutine");
        }
    }


}
