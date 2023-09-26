using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EyeWashCapBehaviourScript : MonoBehaviour
{
    private ParentConstraint parentConstraint;

    private void Start()
    //get the constraint or give warning
    {
        parentConstraint = GetComponent<ParentConstraint>();

        if (parentConstraint == null)
        {
            Debug.LogWarning("ParentConstraint component not found on the GameObject.");
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
}
