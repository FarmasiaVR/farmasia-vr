using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class connectHose : MonoBehaviour

{
    public GameObject objectToConnect;
    public GameObject targetObject;
    public GameObject hoseTwistOne;
    public GameObject hoseTwistTwo;
    public GameObject hoseJointOne;
    public GameObject hoseJointTwo;

    public void Start()
    {
        if (hoseTwistOne == null || hoseTwistTwo == null || hoseJointTwo == null || hoseJointTwo == null)
        {
            Debug.LogWarning("Please assign hose location parameters in the Inspector.");
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

    }
}
