using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class toggleHoverMeshOnTag : MonoBehaviour
{
    public XRSocketInteractor targetSocketInteractor;
    public List<string> enableHoverMeshTags = new List<string>();

    public void toggleHoverMesh(HoverEnterEventArgs args)
    {
        XRGrabInteractable grabInteractable = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        if (grabInteractable)
        {
            bool enableMesh = hoveredObjectHasSelectingInteractorWithTag(grabInteractable);
            if (enableMesh)
            {
                targetSocketInteractor.showInteractableHoverMeshes = true;
            }
            else
            {
                targetSocketInteractor.showInteractableHoverMeshes = false;
            }
           
        }
    }

    bool hoveredObjectHasSelectingInteractorWithTag(XRGrabInteractable grabInteractable)
    {
        
        bool enableMesh = false;
        IXRSelectInteractor selectingInteractable = grabInteractable.firstInteractorSelecting;
        if (selectingInteractable != null)
        {
            enableHoverMeshTags.ForEach(tag => {
                if(selectingInteractable.transform.gameObject.CompareTag(tag))
                {
                    enableMesh = true;
                }
            });
            
        }

        return enableMesh;
    }
}
