using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlanketEndCanvas : MonoBehaviour
{
    public GameObject canvasObject;

    public void ActivateEnd()
    {
        if (canvasObject != null)
        {
            canvasObject.SetActive(true);
        }
        else
        {
            Debug.LogError("object reference is missing.");
        }
    }
}
