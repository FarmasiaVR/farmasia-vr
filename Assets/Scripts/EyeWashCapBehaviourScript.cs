using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
//this script controls be working of the eyewashbottle after the cap is grabbed.

public class EyeWashCapBehaviourScript : MonoBehaviour
{
    //keeps the cap attached, must be disabled so cap doesnt follow bottle movement.
    private ParentConstraint parentConstraint;
    public ParticleSystem particleSystemToActivate;
    //collider to be possibly used for an eyewashing task.
    public Collider colliderToActivate;

    private void Start()
    //starting parameters adjusted
    {
        parentConstraint = GetComponent<ParentConstraint>();

        if (parentConstraint == null)
        {
            Debug.LogWarning("ParentConstraint component not found on the GameObject.");
        }

        if (particleSystemToActivate != null)
        {
            particleSystemToActivate.Stop();
        }

        if (colliderToActivate != null)
        {
            colliderToActivate.enabled = false;
        }
    }

    public void DisableConstraint()
    {
        if (parentConstraint != null)
        {
            //deactivation of the parent constraint
            parentConstraint.enabled = false;
        }
    }

    public void TurnOnParticleSystemAndCollider()
    {
        //starting particlesystem
        if (particleSystemToActivate != null)
        {
            particleSystemToActivate.Play();
        }

        //enabling collider
        if (colliderToActivate != null)
        {
            colliderToActivate.enabled = true;
        }
    }

    public void TurnOnParticleSystemAndColliderForDuration(float durationInSeconds)
    {
        StartCoroutine(TurnOnParticleSystemAndColliderCoroutine(durationInSeconds));
    }

    private IEnumerator TurnOnParticleSystemAndColliderCoroutine(float duration)
        //responsible for activating and deactivating system after a duration. Bottles do not tend to have infinite liquid reserves.
    {
        if (particleSystemToActivate != null)
        {
            particleSystemToActivate.Play();
        }

        if (colliderToActivate != null)
        {
            colliderToActivate.enabled = true;
        }

        //waiting for the duration
        yield return new WaitForSeconds(duration);

        if (particleSystemToActivate != null)
        {
            particleSystemToActivate.Stop();
        }

        if (colliderToActivate != null)
        {
            colliderToActivate.enabled = false;
        }
    }
}