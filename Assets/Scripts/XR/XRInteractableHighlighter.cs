using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractableHighlighter : MonoBehaviour
{
    public Color highlightColor;

    private List<XRBaseInteractable> interactablesHovered;
    private Transform highlightedObject;

    private void Start() {
        XRBaseInteractor interactor = GetComponent<XRBaseInteractor>();
        interactor.hoverEntered.AddListener(HoveredEvent);
        interactor.hoverExited.AddListener(ExitHoverEvent);
        interactor.selectEntered.AddListener(SelectedEvent);
        interactor.selectExited.AddListener(SelectExitEvent);
        interactablesHovered= new List<XRBaseInteractable>();
    }

    public void HoveredEvent(HoverEnterEventArgs hoveredArgs) {
        ///Don't highlight selected objects. If they are selected, highlight them only if they are held by a socket.
        ///
        interactablesHovered.Add(hoveredArgs.interactableObject.transform.GetComponent<XRBaseInteractable>());
    }

    public void ExitHoverEvent (HoverExitEventArgs hoveredArgs) {
        //Make sure that the highlight is removed only when no interactors are interacting
        interactablesHovered.Remove(hoveredArgs.interactableObject.transform.GetComponent<XRBaseInteractable>());
    }

    public void SelectedEvent (SelectEnterEventArgs selectedArgs)
    {
        //Make sure that the highlight is turned off when picking up an object.
        interactablesHovered.Remove(selectedArgs.interactableObject.transform.GetComponent<XRBaseInteractable>());
    }

    public void SelectExitEvent (SelectExitEventArgs selectedArgs)
    {
        //When deselecting an object, make it highlightable again.
        //interactablesHovered.Add(selectedArgs.interactableObject.transform.GetComponent<XRBaseInteractable>());
    }

    private void ChangeHiglight(Transform hoveredObject, bool isHighlighting) {
        ///<summary>
        ///When given a hovered object, highlights it. If the hovered object isn't a mesh (i.e an empty), then highlights all of the children
        ///</summary>
        ///<param name="hoveredObject">The object for which the highlight will change</param>
        ///<param name="isHighlighting">Whether the object should be highlighted or not. If true, the object will be highlighted and if false, the object's highlight will be disabled</param>
        Renderer renderer = hoveredObject.GetComponent<Renderer>();
        XRSocketInteractor socket = hoveredObject.GetComponent<XRSocketInteractor>();
        if (renderer != null) {
            ChangeHighlightMesh(renderer, isHighlighting);
        }
        else if (hoveredObject.GetComponent<XRSocketInteractor>())
        {
            //If the player is hovering over a socket then highlight the socketed object
            ChangeHiglight(socket.GetOldestInteractableSelected().transform, isHighlighting);
        }
        else {
            foreach (Transform child in hoveredObject) {
                Renderer childRenderer = child.GetComponent<Renderer>();
                if (childRenderer != null) {
                    ChangeHighlightMesh(childRenderer, isHighlighting);
                }
            }
        }
    }
    
    private void ChangeHighlightMesh(Renderer renderer, bool isHighlight) {
        renderer.material.SetColor("_EmissionColor", highlightColor);
        if (isHighlight) {
            renderer.material.EnableKeyword("_EMISSION");
        } else { renderer.material.DisableKeyword("_EMISSION"); }
    }

    private Transform GetNearestHighlightableInteractable()
    {
        ///<summary>
        /// Goes through every object that is hovered at that point and returns the nearest one.
        /// </summary>
        Transform nearestObject = null;
        float nearestObjectDist = float.MaxValue;

        foreach (XRBaseInteractable interactable in interactablesHovered.ToArray())
        {
            /// If there are interactors interacting with the hovered object, then only highlight it if the interactor is a socket.
            if (interactable.interactorsSelecting.Count > 0)
            {
                if (!interactable.interactorsSelecting[0].transform.GetComponent<XRSocketInteractor>())
                {
                    continue;
                }
            }
            float distanceToInteractable = Vector3.Distance(transform.position, interactable.transform.position);
            if (distanceToInteractable < nearestObjectDist)
            {
                nearestObject = interactable.transform;
                nearestObjectDist = distanceToInteractable;
            }
        }

        return nearestObject;
    }

    private void Update()
    {
        if (highlightedObject)
        {
            ChangeHiglight(highlightedObject, false);
            highlightedObject = null;
        }

        Transform objectToHilghlight = GetNearestHighlightableInteractable();

        if (objectToHilghlight)
        {
            ChangeHiglight(objectToHilghlight, true);
            highlightedObject = objectToHilghlight;
        }
        
    }


}
