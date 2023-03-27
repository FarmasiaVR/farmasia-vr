using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractableHighlighter : MonoBehaviour
{
    public Color highlightColor;

    private List<XRBaseInteractable> interactablesHovered;
    private XRBaseInteractor interactor;
    private Transform highlightedObject;

    private void Start() {
        interactor = GetComponent<XRBaseInteractor>();
        interactor.targetPriorityMode = TargetPriorityMode.All;
    }



    private void ChangeHiglight(Transform hoveredObject, bool isHighlighting) {
        ///<summary>
        ///When given a hovered object, highlights it and also highlight all of the children
        ///</summary>
        ///<param name="hoveredObject">The object for which the highlight will change</param>
        ///<param name="isHighlighting">Whether the object should be highlighted or not. If true, the object will be highlighted and if false, the object's highlight will be disabled</param>
        Renderer renderer = hoveredObject.GetComponent<Renderer>();
        XRSocketInteractor socket = hoveredObject.GetComponent<XRSocketInteractor>();
        if (renderer != null) {
            ChangeHighlightMesh(renderer, isHighlighting);
        }
        /*
         * Refactor and use this if the objects in sockets should also be highlighted.
        else if (socket)
        {
            //If the player is hovering over a socket then highlight the socketed object
            ChangeHiglight(socket.GetOldestInteractableSelected().transform, isHighlighting);
        }
        */

        foreach (Transform child in hoveredObject) {
            //Highlight all of the children using recursion.
            ChangeHiglight(child, isHighlighting);
        }

    }
    
    private void ChangeHighlightMesh(Renderer renderer, bool isHighlight) {
        foreach (Material material in renderer.materials)
        {
            material.SetColor("_EmissionColor", highlightColor);
            if (isHighlight)
            {
                material.EnableKeyword("_EMISSION");
            }
            else { material.DisableKeyword("_EMISSION"); }
        }
    }

    private void Update()
    {
        if (highlightedObject!= null)
        {
            ChangeHiglight(highlightedObject, false);
            highlightedObject= null;
        }

        if (!interactor.isSelectActive && interactor.allowHover)
        {
            // If the player isn't currently selecting anything then go through all of the hovered objects until an object is found that can be highlighted.
            foreach (IXRSelectInteractable selectTarget in interactor.targetsForSelection)
            {
                // If the interactable is currently selected then don't highlight it unless the selecting object is a socket interactor.
                if (!selectTarget.isSelected | (selectTarget.isSelected && selectTarget.interactorsSelecting[0].transform.GetComponent<XRSocketInteractor>())) 
                {
                    ChangeHiglight(selectTarget.transform, true);
                    highlightedObject = selectTarget.transform;
                    break;
                }
            }
        }
    }


}
