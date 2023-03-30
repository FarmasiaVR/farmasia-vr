using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractableHighlighter : MonoBehaviour
{
    public Color highlightColor;

    private XRBaseInteractor interactor;
    private List<Transform> highlightedObjects = new List<Transform>();

    private void Start() {
        interactor = GetComponent<XRBaseInteractor>();
        interactor.targetPriorityMode = TargetPriorityMode.All;

        interactor.hoverExited.AddListener(args => { ChangeHiglight(args.interactableObject.transform, false); });
        interactor.selectEntered.AddListener(args => { ChangeHiglight(args.interactableObject.transform, false); });
    }



    private void ChangeHiglight(Transform hoveredObject, bool isHighlighting) {
        ///<summary>
        ///When given a hovered object, highlights it and also highlight all of the children
        ///</summary>
        ///<param name="hoveredObject">The object for which the highlight will change</param>
        ///<param name="isHighlighting">Whether the object should be highlighted or not. If true, the object will be highlighted and if false, the object's highlight will be disabled</param>
        Renderer renderer = hoveredObject.GetComponent<Renderer>();
        XRSocketInteractor socket = hoveredObject.GetComponent<XRSocketInteractor>();
        if (renderer != null && hoveredObject.gameObject.activeInHierarchy) {
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
        foreach (Transform highlightedObject in highlightedObjects.ToArray())
        {
            ChangeHiglight(highlightedObject, false);
            highlightedObjects.Remove(highlightedObject);
        }

        if (!interactor.isSelectActive && interactor.allowHover && interactor.targetsForSelection.Count>0)
        {
            XRBaseInteractable highlightCanditate = interactor.targetsForSelection[0] as XRBaseInteractable;
            // If the interactable is currently selected then don't highlight it unless the selecting object is a socket interactor.
            if (!highlightCanditate.isSelected | (highlightCanditate.isSelected && highlightCanditate.interactorsSelecting[0].transform.GetComponent<XRSocketInteractor>())) 
            {
                ChangeHiglight(highlightCanditate.transform, true);
                highlightedObjects.Add(highlightCanditate.transform);
            }
        }
    }


}
