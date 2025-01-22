using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

//This script bends a hose at the start of a game to it's proper position. This way the hose object can be straight without need to bend it
//manually when creating an object including a bendable hose.

public class connectHose : MonoBehaviour

{
    //The end point object and the hose end must be given. These objects should be connected to each other with some kind of joint module.
    //There are also gameobjects which's locations adjust the hose bending shape.
    //The script does not currently support varying amount of these points but should be reasonably easy to chance if the need arises.
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
