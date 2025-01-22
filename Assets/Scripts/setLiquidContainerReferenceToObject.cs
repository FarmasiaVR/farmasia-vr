using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class setLiquidContainerReferenceToObject : MonoBehaviour
{

    public FilteringButton target;
    
    public void setLiquidRef(SelectEnterEventArgs args)
    {
        FilterCurrentAttachedTank referrer = args.interactableObject.transform.gameObject.GetComponent<FilterCurrentAttachedTank>();
        if(referrer != null)
        {
            LiquidContainer container = referrer.tanksLiguidContainer;
            target.Container= container;
        }
    }

    public void setLiquidRefToNull(SelectExitEventArgs args)
    {
        target.Container = null;
    }
}
