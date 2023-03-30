using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class toggleInteractionLayerMaskXR : MonoBehaviour
{

    public XRBaseInteractable socketsOwnerInteractable;

    // intertactionLayers is a 32 bit bitfield where the layers are stored as bits, good luck
    //0000 0000 0000 0000 0000 0001 = decimal: 1 = layer at index 1: no
    //0000 0000 0000 0000 0000 0010 = decimal: 2 = the layer at index 2: 
    //0000 0000 0000 0001 0000 0000 = decimal: 256 = can attach to luerlock, layer at index 8
    //i know theres a function to do this easily TM, but it will be left as an exercise for the next developer =)
    int canAttachToLuerlockBitField = 256;
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
