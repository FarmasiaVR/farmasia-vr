using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is a utility class that redirects collision events through UnityEvents.
/// This is useful when you want to access collision events from a script that doesn't have a collider
/// </summary>
public class CollisionEventRedirector : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnter;
    public UnityEvent<Collider> onTriggerExit;
    public UnityEvent<Collision> onCollisionEnter;
    public UnityEvent<Collision> onCollisionExit;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit.Invoke(collision);
    }
}
