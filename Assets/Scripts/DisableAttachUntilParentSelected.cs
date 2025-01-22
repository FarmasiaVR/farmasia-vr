using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableAttachUntilParentSelected : MonoBehaviour
{
    public XRSocketInteractor targetSocket;
    public XRGrabInteractable parent;
    // Start is called before the first frame update
    void Start()
    {
        targetSocket.allowSelect = false;
    }

   
    
    public void disableAttach()
    {
        if (!targetSocket.hasSelection)
        {
            targetSocket.allowSelect = false;
        }
    }
    
    
    public void enableAttach()
    {
        targetSocket.allowSelect = true;
    }


    public void disableAttachIfParentNotSelected()
    {
        if(parent.interactorsSelecting.Count <= 0)
        {
            disableAttach();
        }
    }

}
