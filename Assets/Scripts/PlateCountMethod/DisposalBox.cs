using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposalBox : MonoBehaviour
{
    public AudioSource audioSource;
    void Awake()
    {        
        audioSource = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the tag "BlueStick"
        Debug.Log("Triggered!");
        if (other.CompareTag("Blue Stick"))
        {
            audioSource.Play();
            // Destroy the blue stick object
            Destroy(other.gameObject);
            Debug.Log("Blue Stick disposed!");
        }
    }
}

