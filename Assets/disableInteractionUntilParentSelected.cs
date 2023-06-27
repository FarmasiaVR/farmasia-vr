using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class disableInteractionUntilParentSelected : MonoBehaviour
{
    XRGrabInteractable attachedInteractable;
    public InteractionLayerMask maskBefore;
    public InteractionLayerMask maskAfterDisabling;
    public bool objectGrabbed;
    public bool mustBeSelectedByPlayer;

    public void RestoreInteraction(SelectEnterEventArgs args)
    {
        XRBaseController isSelectedByController = args.interactorObject.transform.GetComponent<XRBaseController>();
        if (mustBeSelectedByPlayer && isSelectedByController == null) 
        {
            return;
        }

        if (attachedInteractable)
        {
            Debug.Log("enabled grab!");
            attachedInteractable.interactionLayers = maskBefore;
        }
        objectGrabbed = true;
        
    }

    public void DisableInteraction()
    {
        if (attachedInteractable)
        {
            Debug.Log("disabled interaction!");
            attachedInteractable.interactionLayers = maskAfterDisabling;
        }
        objectGrabbed = false;

    }

    //Called when interactable is attached to a socket
    public void FirstDisableInteraction(SelectEnterEventArgs args)
    {
       
        attachedInteractable = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        maskBefore = attachedInteractable.interactionLayers;
        if (!objectGrabbed)
        {
            DisableInteraction();
        }
    }

    //Called when interactable is attached to a socket
    public void restoreState(SelectExitEventArgs args)
    {
        attachedInteractable = null;
        maskBefore = 0;
    }
}
