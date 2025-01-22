using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class toggleInteractionLayerMaskXR : MonoBehaviour
{

    public XRBaseInteractable socketsOwnerInteractable;

  
    int canAttachToLuerlockBitField;
    
    int interactionLayerDefault;


    void Start()
    {
        string[] canAttachToLuerlockMask = { "CanAttachToLuerlock", "InteractableByPlayer" };
        canAttachToLuerlockBitField = InteractionLayerMask.GetMask(canAttachToLuerlockMask);
        string[] interactionLayerDefaultMask = { "InteractableByPlayer", "default" };
        interactionLayerDefault = InteractionLayerMask.GetMask(interactionLayerDefaultMask);



    }

    public void setInteractionLayerToDefault()
    {
        socketsOwnerInteractable.interactionLayers = interactionLayerDefault;
    }

    public void setInteractionLayerToCanAttachToLuerlock()
    {
        socketsOwnerInteractable.interactionLayers = canAttachToLuerlockBitField;
    }
}
