using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.VFX;
using UnityEngine.Events;
using UnityEditor;

public class SimpleFire : MonoBehaviour, ITogglableFire {
    [Tooltip("The time (sec) it takes for the extinguish animation to finish")]
    public float extinguishTime = 3;
    [Tooltip("The time (sec) it takes for the ignite animation to finish")]
    public float igniteTime = 6;
    public bool startOnFire = false;
    public UnityEvent onExtinguish = new UnityEvent();
    public UnityEvent onIgnite = new UnityEvent();
    [HideInInspector] public bool isBurning { get; private set; }

    // Visual effects and lighting
    [SerializeField] private VisualEffect fireVFX;
    [SerializeField] private VisualEffect smokeVFX;
    [SerializeField] private VisualEffect ashVFX;
    [SerializeField] private ParticleSystem igniteParticle;
    [SerializeField] private ParticleSystem extinguishParticle;
    [SerializeField] private GameObject pointLight;
    [SerializeField] private AudioSource burningSound;
    [SerializeField] private AudioSource extinguishSound;

    // Extinguish/ignite animation
    private enum ScalingState {Stopped, Igniting, Extinguishing}
    private float scalingAnimTimer = 0;
    private Vector3 origScale = new Vector3(0, 0, 0);
    // Animation state used for extinguishing/ignition size change animation
    private ScalingState currScalingState = ScalingState.Stopped;
    // Used for changing the scaling animation time when calling ignite/extinguish
    private float currScalingAnimTime = 0;

    /// <summary>
    /// Callable method to stop the visual effect animation, turn off the light and to play the extinguishing particle effect.
    /// </summary>
    public void Extinguish(bool playExtinguishAudio, float setExtinguishTime) {
        if (isBurning && currScalingState == ScalingState.Stopped) {
            if (extinguishParticle != null && setExtinguishTime != 0.0f) // Animation time of 0.0 can be used to skip particles and scaling animation
                extinguishParticle.Play();
            if (playExtinguishAudio)
                extinguishSound.Play();
            if (smokeVFX != null)
                smokeVFX.SetFloat("Spawn Rate", 0f);
            if (ashVFX != null)
                ashVFX.SetFloat("Spawn Rate", 0.0f);
            pointLight.SetActive(false);

            isBurning = false;
            currScalingAnimTime = setExtinguishTime;
            currScalingState = ScalingState.Extinguishing;
            onExtinguish.Invoke();
        }
    }

    public void Extinguish() { Extinguish(true, extinguishTime); }

    /// <summary>
    /// Callable method to play the visual effect animation, turn on the light and to play the ignition particle effect.
    /// </summary>
    public void Ignite(float setIgniteTime) {
        if (!isBurning && currScalingState == ScalingState.Stopped) {
            if (igniteParticle != null && setIgniteTime != 0.0f) // Animation time of 0.0 works just like in Extinguish
                igniteParticle.Play();
            if (smokeVFX != null)
                smokeVFX.SetFloat("Spawn Rate", 15f);
            if (ashVFX != null)
                ashVFX.SetFloat("Spawn Rate", 120.0f);
            fireVFX.Play();
            burningSound.Play();
            pointLight.SetActive(true);

            isBurning = true;
            currScalingAnimTime = setIgniteTime;
            currScalingState = ScalingState.Igniting;
            onIgnite.Invoke();
        }
    }

    public void Ignite() { Ignite(igniteTime); }

    public void OnEnable() {
        origScale = fireVFX.transform.localScale;
        if (startOnFire) {
            Ignite(0.0f);
        } else {
            isBurning = true;
            Extinguish(false, 0.0f);
        }
    }

    private void FinishAnimation() {
        currScalingState = ScalingState.Stopped;
        scalingAnimTimer = 0;
    }

    public void Update() {
        if (currScalingState != ScalingState.Stopped) {
            scalingAnimTimer += Time.deltaTime;

            if (currScalingState == ScalingState.Igniting) {
                if (scalingAnimTimer < currScalingAnimTime) {
                    // Ignite animation in progress 
                    fireVFX.transform.localScale = origScale * (scalingAnimTimer / currScalingAnimTime);
                } else {
                    // Ignition finished
                    fireVFX.transform.localScale = origScale;
                    FinishAnimation();
                }
            } else if (currScalingState == ScalingState.Extinguishing) {
                if (scalingAnimTimer < currScalingAnimTime) {
                    // Extinguish animation in progress
                    fireVFX.transform.localScale = origScale * (1 - scalingAnimTimer / currScalingAnimTime);
                } else {
                    // Extinguish animation finished
                    fireVFX.transform.localScale = new Vector3(0, 0, 0);
                    fireVFX.Stop();
                    burningSound.Stop();
                    FinishAnimation();
                }
            }
        }
    }
}