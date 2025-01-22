using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSparePartsCabinet : MonoBehaviour
{
    public GameObject cabinetObject;
    public GameObject buttonObject;
    public void ToggleSpareparts()
    {
        if (cabinetObject != null && buttonObject != null)
        {
            cabinetObject.SetActive(true);
            buttonObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Cabinet object reference is missing.");
        }
    }
}
