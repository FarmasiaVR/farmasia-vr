using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.VFX;
using UnityEngine.Events;
// This will add a new particle system to FireGridObject, not necessary now
//[RequireComponent(typeof(ParticleSystem))]

public class FireGrid : MonoBehaviour
{
    // Different particle effect fields
    [SerializeField]
    private VisualEffect fireVFX;
    [SerializeField]
    private VisualEffect smokeVFX;
    [SerializeField]
    private ParticleSystem igniteParticle;
    [SerializeField]
    private ParticleSystem extinguishParticle;

    // Audio clip variables to store audio
    [SerializeField]
    private AudioClip extinguishAudio;
    [SerializeField]
    private AudioClip extinguishAudioBlanket;

    // Temperature of the fire
    [SerializeField]
    private int degrees;
    
    // Light source of the fire
    [SerializeField]
    private GameObject pointLight;

    // Collider tile-shaped cube for collision detection
    [SerializeField]
    private GameObject colliderCube;

    // Delays ignition by seconds
    [SerializeField]
    public float ignitionTimer;
    
    // Adds reference to the FireCounter script
    public FireCounter fireCounter;

    public UnityEvent onExtinguish = new UnityEvent();

    // Key used to manually trigger the ignition of the fire
    public KeyCode igniteKey = KeyCode.Space;

    // Key used to manually trigger the extinguishing of the fire
    public KeyCode extinguishKey = KeyCode.G;

    // Adds reference to the FlameExtinguish script
    private FlameExtinguish flameExtinguish;

    // True if the fire is active, false if it has been extinguished.
    private bool isIgnited;


    private void Awake()
    {
        // Finds FlameExtinguish component
        flameExtinguish = GetComponent<FlameExtinguish>();

        // Finds FireCounter component
        if (fireCounter == null)
        {
            fireCounter = GameObject.Find("FireCounter").GetComponent<FireCounter>();
        }

        // Stop the fireVFX in the Awake method to ensure it doesn't start playing immediately.
        // This is necessary if you intend to delay the start of the VFX animation.
        fireVFX.Stop();
    }

    /// <summary>
    /// Starts the IgnitionDelay Coroutine to delay the fire ignition based on the time specified in ignitionTimer.
    /// Increment the fire count by 1 to keep track of the total number of active fires.
    /// </summary>
    private void Start()
    {
        // Start the IgnitionDelay coroutine to handle ignition after a delay
        StartCoroutine(IgnitionDelay());

        // Increments fire count by 1
        if (fireCounter)
        {
            fireCounter.IncrementFireCount();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // FOR TESTING IGNITION AND EXTINGUISHING - Press Space for ignition and G for extinguishing
        if (Input.GetKeyDown(igniteKey))
        {
            Ignite();
        //Debug.Log("Fire should ignite");
        }
        if(Input.GetKeyDown(extinguishKey))
        {
            Extinguish();
            //Debug.Log("Fire should extinguish");
        }
    }

    /// <summary>
    /// Initiates the fire ignition after the duration(seconds) specified in the ignitionTimer variable, 
    /// which can be set through the Inspector window.
    /// </summary>
    IEnumerator IgnitionDelay()
    {
        yield return new WaitForSeconds(ignitionTimer);
        Ignite();
    }

    /// <summary>
    /// Callable method to stop the visual effect animation, turn off the light and to play the extinguishing particle effect.
    /// </summary>
    public void Extinguish()
    {
        if (isIgnited)
        {
            AudioSource fireAudioSource = fireVFX.gameObject.GetComponent<AudioSource>();
            fireAudioSource.Stop();
            fireAudioSource.clip = extinguishAudio;
            fireAudioSource.loop = false;
            fireAudioSource.Play();
            
            if (smokeVFX)
            {
                smokeVFX.SetFloat("Spawn Rate", 0f);
            }
            pointLight.SetActive(false);

            // Runs flameExtinguish script and Stops the FireGrid VFX
            if (flameExtinguish)
            {
                flameExtinguish.enabled = true;
            }
            else
            {
                Debug.Log("No flameExtinguish script found!");
            }

            // Decrement fire count by 1
            if (fireCounter)
            {
                fireCounter.DecrementFireCount();
            }
            else
            {
                Debug.Log("No FireCounter found in the scene!");
            }
            isIgnited = false;
            onExtinguish.Invoke();
            Debug.Log("Extinguished");
        }
        
    }

    /// <summary>
    /// Callable method to stop the visual effect animation by fire blanket, turn off the light and to play the extinguishing particle effect.
    /// </summary>
    public void ExtinguishWithBlanket()
    {
        if (isIgnited)
        {
            AudioSource fireAudioSource = fireVFX.gameObject.GetComponent<AudioSource>();
            fireAudioSource.Stop();
            fireAudioSource.clip = extinguishAudioBlanket;
            fireAudioSource.loop = false;
            fireAudioSource.Play();
            fireVFX.Stop();
            if (smokeVFX)
            {
                smokeVFX.SetFloat("Spawn Rate", 0f);
            }
            pointLight.SetActive(false);

            if (extinguishParticle != null)
            {
                extinguishParticle.Play();
            }

            onExtinguish.Invoke();
            isIgnited = false;
            //Debug.Log("Extinguished with Blanket");
        }
        
    }

    /// <summary>
    /// Callable method to play the visual effect animation, turn on the light and to play the ignition particle effect.
    /// </summary>
    public void Ignite()
    {
        if (!isIgnited)
        {
            fireVFX.Play();
            if (smokeVFX)
            {
                smokeVFX.SetFloat("Spawn Rate", 15f);
            }
            pointLight.SetActive(true);
            
            if (igniteParticle != null)
            {
                igniteParticle.Play();
            }

            isIgnited = true;
            //Debug.Log("Ignited");
        }
        
    }

    /// <summary>
    /// Returns the value of bool isIgnited that gets turned to true when Ignite() is ran.
    /// </summary>
    /// <returns></returns>
    public bool IsIgnited()
    {
        return isIgnited;
    }
}
