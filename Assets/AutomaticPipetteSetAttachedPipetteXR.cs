using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AutomaticPipetteSetAttachedPipetteXR : MonoBehaviour
{

    public BigPipetteXR reiceiverPipette;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    //called when interactable is detached from a socket
    public void setConnectedPipette(SelectEnterEventArgs args)
    {
        //Debug.Log("setting connected pipette");
        //MUHAHAHAHAHAHAHHAHAH this line of code is cracy but you can do better so fix this???
        reiceiverPipette.pipetteContainerXR = args.interactableObject.transform.GetComponent<PipetteContainer>();
    }

    public void setConnectedPipetteNull(SelectExitEventArgs args)
    {
        reiceiverPipette.pipetteContainerXR = null;
    }

}
