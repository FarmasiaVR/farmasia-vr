using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class toggleInteractionLayerMaskXR : MonoBehaviour
{

    public XRBaseInteractable socketsOwnerInteractable;

    
    int canAttachToLuerlockBitField = InteractionLayerMask.GetMask("CanAttachToLuerlock");
    int interactionLayerDefault = 1;

    void Start()
    {
      

        
        
       
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
