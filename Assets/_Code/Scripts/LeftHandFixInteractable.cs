using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class LeftHandFixInteractable : XRGrabInteractable
{
    public Transform leftAttach;
    public Transform rightAttach;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Controller (Left)"))
        {
            attachTransform = leftAttach;
        }
        else if (args.interactorObject.transform.CompareTag("Controller (Right)"))
        {
            attachTransform = rightAttach;
        }

        base.OnSelectEntering(args);
    }
}
