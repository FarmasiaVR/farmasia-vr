using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractableHighlighter : MonoBehaviour
{
    public Color highlightColor;

    private void Start() {
        XRBaseInteractor interactor = GetComponent<XRBaseInteractor>();
        interactor.hoverEntered.AddListener(HoveredEvent);
        interactor.hoverExited.AddListener(ExitHoverEvent);
        interactor.selectEntered.AddListener(SelectedEvent);
    }

    public void HoveredEvent(HoverEnterEventArgs hoveredArgs) {
        ///Don't highlight selected objects. If they are selected, highlight them only if they are held by a socket.
        ///

        bool hoveredObjectIsSelected = false;
        bool selectingObjectIsSocket = false;

        List<IXRSelectInteractor> interactorsSelecting = hoveredArgs.interactableObject.transform.GetComponent<XRBaseInteractable>().interactorsSelecting;

        if (interactorsSelecting.Count > 0) {
            hoveredObjectIsSelected = true;
            if (interactorsSelecting[0].transform.GetComponent<XRSocketInteractor>()) {
                selectingObjectIsSocket = true;
            }
        }
  


        if (!hoveredObjectIsSelected | selectingObjectIsSocket) {
            ChangeHiglight(hoveredArgs.interactableObject.transform, true);
        }
        

    }

    public void ExitHoverEvent (HoverExitEventArgs hoveredObject) {
        //Make sure that the highlight is removed only when no interactors are interacting
        ChangeHiglight(hoveredObject.interactableObject.transform, false);
    }

    public void SelectedEvent (SelectEnterEventArgs selectedObject)
    {
        //Make sure that the highlight is turned off when picking up an object.
        ChangeHiglight(selectedObject.interactableObject.transform, false);
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

   
}
