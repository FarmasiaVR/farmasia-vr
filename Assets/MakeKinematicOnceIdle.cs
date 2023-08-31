using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MakeKinematicOnceIdle : MonoBehaviour
{
    XRGrabInteractable targetInteractable;
    Rigidbody targetRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        targetRigidBody = gameObject.GetComponent<Rigidbody>();
        if (!targetRigidBody)
        {
            Debug.LogError("MakeKinematicOnceIdle didnt find Rigidbody on gameobject!");
        }


        targetInteractable = gameObject.GetComponent<XRGrabInteractable>();
        if (!targetInteractable)
        {
            Debug.LogError("MakeKinematicOnceIdle didnt find XRGrabInteractable on gameobject!");
        }
    }
    
    //this FixedUpdate makes the target rigidbody kinematic if it is sleeping and target interactable is not selected
    private void FixedUpdate()
    {
        if (targetRigidBody && targetInteractable)
        {
            //IsSleeping returns true when the rigidbody moves slower than Sleep Treshold variable determined in the Physics settings
            //read more at: https://docs.unity3d.com/Manual/RigidbodiesOverview.html
            if (targetRigidBody.IsSleeping())
            {
                if (!targetInteractable.isSelected)
                {
                    targetRigidBody.isKinematic = true;
                }
            }
        }
    }

}
