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
    public void RestoreInteraction()
    {
        if (attachedInteractable)
        {
            Debug.Log("enabled grab!");
            attachedInteractable.interactionLayers = maskBefore;
        }
    }

    public void DisableInteraction()
    {
        if (attachedInteractable)
        {
            Debug.Log("disabled interaction!");
            attachedInteractable.interactionLayers = maskAfterDisabling;
        }
        
    }

    //Called when interactable is attached to a socket
    public void FirstDisableInteraction(SelectEnterEventArgs args)
    {
        
        attachedInteractable = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        maskBefore = attachedInteractable.interactionLayers;
        DisableInteraction();
    }

    //Called when interactable is attached to a socket
    public void restoreState(SelectExitEventArgs args)
    {
        attachedInteractable = null;
        maskBefore = 0;
    }
}
