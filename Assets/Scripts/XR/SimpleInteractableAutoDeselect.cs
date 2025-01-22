using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This is a script that is used to automatically deselect a simple interactable when it is selected.
/// Otherwise, when selecting a simple interactable (like a button), the player would need to deselect it before selecting another object.
/// </summary>
public class SimpleInteractableAutoDeselect : MonoBehaviour
{
    public float selectionCooldown = 0.5f;

    private void Start()
    {
        XRBaseInteractor baseInteractor = GetComponent<XRBaseInteractor>();
        baseInteractor.selectEntered.AddListener(AutoExitSelect);
    }

    public void AutoExitSelect(SelectEnterEventArgs args)
    {
        //Debug.Log("Selected object " + args.interactableObject.transform.name);
        StartCoroutine(DeselectAfterCooldown(args));
    }

    public IEnumerator DeselectAfterCooldown(SelectEnterEventArgs args)
    {
        ///Use a coroutine since deselecting a simple interactable immediately after selecting it may cause the player to select an unwanted object.
        bool selectedObjectIsSimple = (args.interactableObject.transform.GetComponent<XRSimpleInteractable>() != null);

        if (selectedObjectIsSimple)
        {
            // As it turns out, the SelectEnterEvent may change during WaitForSeconds. As such, make sure that only drop the item if the item held is the same that was selected.

            IXRSelectInteractable interactable = args.interactableObject;
            IXRSelectInteractor interactor = args.interactorObject;

            yield return new WaitForSeconds(selectionCooldown);
            XRInteractionManager interactionManager = args.manager;

            if (interactable.interactorsSelecting.Count > 0)
            {
                if (interactable.interactorsSelecting[0] == interactor) {
                    interactionManager.SelectExit(args.interactorObject, args.interactableObject);
                }
            }

            // In some instances (i.e when opening a cover for an object), the interactor may select the empty object that was supposed to be deleted.
            // This may be a bug with the XR library, but to avoid this make sure that the object the interactor is selecting actually exists.

            if (interactor.isSelectActive)
            {
                if (!interactor.firstInteractableSelected.transform.gameObject.activeSelf)
                {
                    interactionManager.SelectExit(interactor, interactor.firstInteractableSelected);
                }
            }
        }
    }
}
