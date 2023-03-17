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
        interactor.targetPriorityMode = TargetPriorityMode.HighestPriorityOnly;
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

    private void Update()
    {
        if (highlightedObject!= null)
        {
            ChangeHiglight(highlightedObject, false);
            highlightedObject= null;
        }
        if (interactor.targetsForSelection.Count > 0 && !interactor.isSelectActive && interactor.allowHover)
        {
            Transform highestPriorityObject = interactor.targetsForSelection[0].transform;
            ChangeHiglight(highestPriorityObject, true);
            highlightedObject = highestPriorityObject;
        }
    }


}
