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
    private void Start()
    {
        XRBaseInteractor baseInteractor = GetComponent<XRBaseInteractor>();
        baseInteractor.selectEntered.AddListener(AutoExitSelect);
    }

    public void AutoExitSelect(SelectEnterEventArgs args)
    {
        bool selectedObjectIsSimple = args.interactableObject.transform.GetComponent<XRSimpleInteractable>();

        if (selectedObjectIsSimple)
        {
            XRInteractionManager interactionManager = args.manager;

            interactionManager.SelectExit(args.interactorObject, args.interactableObject);
        }
    }
}
