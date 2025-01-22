using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanChildItems : MonoBehaviour
{
    public bool cleanAll;
    // This script when attached to a gameobject cleans all childobjects that are attached to it if cleanAll = true
    void Awake()
    {
        if (cleanAll)
        {
            GeneralItem[] generalItems = gameObject.GetComponentsInChildren<GeneralItem>();


            foreach (GeneralItem item in generalItems)
            {
                item.Contamination = GeneralItem.ContaminateState.Clean;
            }
        }
    }

    
}
