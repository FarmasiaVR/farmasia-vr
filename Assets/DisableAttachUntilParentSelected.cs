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

    //this should be called by the socket onSelectExit, and by parent onSelectEnter/onSelectExit
    public void updateAttach()
    {
        //prevent socket losing selected object if socket already selected something
        if (targetSocket.hasSelection)
        {
            return;
        }

        if (parent.firstInteractorSelecting.transform.GetComponent<XRBaseController>())
        {
            targetSocket.allowSelect = true;
        }
        else
        {
            targetSocket.allowSelect = false;
        }
    }

}
