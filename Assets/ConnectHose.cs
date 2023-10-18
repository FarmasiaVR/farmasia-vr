using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class connectHose : MonoBehaviour

{
    private ParentConstraint parentConstraint;
    public GameObject objectToConnect;
    public GameObject targetObject;
    public GameObject hoseTwistOne;
    public GameObject hoseTwistTwo;
    public GameObject hoseJointOne;
    public GameObject hoseJointTwo;

    private void Start()
    {
        if (hoseTwistOne == null || hoseTwistTwo == null || hoseJointTwo == null || hoseJointTwo == null)
        {
            Debug.LogWarning("Please assign hose location parameters.");
            return;
        }

        if (objectToConnect == null || targetObject == null)
        {
            Debug.LogWarning("Please assign objectToConnect and targetObject in the Inspector.");
            return;
        }

        // Move the objectToConnect to the targetObject's position and bend hose.

        objectToConnect.transform.position = targetObject.transform.position;

        hoseJointOne.transform.position = hoseTwistOne.transform.position;
        hoseJointTwo.transform.position = hoseTwistTwo.transform.position;

        // Get and activate the parent constraint.
        parentConstraint = objectToConnect.GetComponent<ParentConstraint>();

        if (parentConstraint != null)
        {
            parentConstraint.enabled = true;
        }
        else
        {
            Debug.LogWarning("No ParentConstraint component found on the objectToConnect.");
        }
    }
}
