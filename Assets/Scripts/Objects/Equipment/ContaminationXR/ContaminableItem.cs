using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContaminableItem : MonoBehaviour
{
    public bool dirty;

    [Header("Events")]

    [Tooltip("Called when the item is cleaned")]
    public UnityEvent onCleaned;

    [Tooltip("Called when the item is contaminated")]
    public UnityEvent onContaminated;


    public void SetClean()
    {
        if (dirty)
        {
            dirty = false;
            onCleaned.Invoke();
        }
    }

    public void SetDirty()
    {
        if (!dirty)
        {
            dirty = true;
            onContaminated.Invoke();
        }
    }
}
