using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipToAsepticSpaceButton : MonoBehaviour
{

    public GameObject helperObject; // Reference to the GameObject with the ManualTester'sLilHelper component

    // This method will be called from the button
    public void ActivateManualTesterHelper()
    {
        if (helperObject != null)
        {
            helperObject.SetActive(true); 
        }
        else
        {
            Debug.LogError("Helper object reference is missing.");
        }
    }
}
