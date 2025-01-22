using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FilterCurrentAttachedTank : MonoBehaviour
{
    //this class exists for the sole purpose of finding the attached liguid container in a tank that is attached to a filters base
    //once the filter is attached to a pump the tanksLiguidContainer is passed to the pumps button,
    //which needs the liquid container for removing the liquid from the tank

    public LiquidContainer tanksLiguidContainer;


    public void findLiguidContainerIfPresent(SelectEnterEventArgs args)
    {
        tanksLiguidContainer = args.interactableObject.transform.gameObject.GetComponentInChildren<LiquidContainer>();
        if(tanksLiguidContainer)
        {
            Debug.Log("found liquid ");
        }
    }

    public void setLiguidContainerToNull(SelectExitEventArgs args)
    {
        tanksLiguidContainer = null;
    }
}
