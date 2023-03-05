using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.VFX;
using Codice.Client.Common.GameUI;
// This will add a new particle system to FireGridObject, not necessary now
//[RequireComponent(typeof(ParticleSystem))]

public class FireGrid : MonoBehaviour
{
    public KeyCode igniteKey = KeyCode.Space;
    public KeyCode extinguishKey = KeyCode.G;
    //public InputActionReference igniteEvent;
    // Different particle effect fields
    [SerializeField]
    private VisualEffect fireVFX;
    [SerializeField]
    private VisualEffect smokeVFX;
    [SerializeField]
    private ParticleSystem igniteParticle;
    [SerializeField]
    private ParticleSystem extinguishParticle;
    
    // Light source of the fire
    [SerializeField]
    private GameObject pointLight;

    // Collider tile-shaped cube for collision detection
    [SerializeField]
    private GameObject colliderCube;

    [SerializeField]
    private AudioClip extinguishAudio;

    private bool isIgnited;

    public bool igniteOnStart;
    
    [SerializeField]
    private int degrees;


    private void Start()
    {
        if (igniteOnStart)
        {
            Debug.Log("igniteOnStart is set to " + igniteOnStart + " inside if-condition");
            Ignite();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // FOR TESTING IGNITION AND EXTINGUISHING - Press Space for ignition and G for extinguishing
        if (Input.GetKeyDown(igniteKey))
        {
            Ignite();
        Debug.Log("Fire should ignite");
        }
        if(Input.GetKeyDown(extinguishKey))
        {
            Extinguish();
            Debug.Log("Fire should extinguish");
        }
    }

    /// <summary>
    /// Callable method to stop the visual effect animation, turn off the light and to play the extinguishing particle effect.
    /// </summary>
    public void Extinguish()
    {
        fireVFX.Stop();
        AudioSource fireAudioSource = fireVFX.gameObject.GetComponent<AudioSource>();
        fireAudioSource.Stop();
        fireAudioSource.clip= extinguishAudio;
        fireAudioSource.loop = false;
        fireAudioSource.Play();
        if (smokeVFX)
        {
            smokeVFX.SetFloat("Spawn Rate", 0f);
        }
        pointLight.SetActive(false);
        if (extinguishParticle != null && isIgnited == true)
        {
            extinguishParticle.Play();
            isIgnited = false;
        }
        Debug.Log("Extinguished");
    }

    /// <summary>
    /// Callable method to play the visual effect animation, turn on the light and to play the ignition particle effect.
    /// </summary>
    public void Ignite()
    {
        fireVFX.Play();
        if (smokeVFX)
        {
            smokeVFX.SetFloat("Spawn Rate", 50f);
        }
        pointLight.SetActive(true);
        if (igniteParticle != null && isIgnited == false)
        {
            igniteParticle.Play();
            isIgnited = true;
        }
        Debug.Log("Ignited");
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
