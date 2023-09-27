using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EyeWashCapBehaviourScript : MonoBehaviour
{
    private ParentConstraint parentConstraint;
    public ParticleSystem particleSystemToActivate;
    public Collider colliderToActivate;

    private void Start()
    //get the constraint or give warning
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