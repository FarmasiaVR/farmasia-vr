using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorSwitcher : MonoBehaviour
{
    [Space]
    [Header("Interactor Objects")]

    [SerializeField]
    private XRBaseInteractor rayInteractor;
    [SerializeField]
    private XRBaseInteractor directInteractor;
    [SerializeField]
    private XRBaseInteractor teleportInteractor;

    [Space]
    [Header("Input Events")]

    [SerializeField]
    private InputActionReference rayInteractorActivate;
    [SerializeField]
    private InputActionReference teleportActivate;

    private List<IXRSelectInteractable> interactablesSelected;




    private void Start() {
        rayInteractorActivate.action.started += EnableRayInteractor;
        rayInteractorActivate.action.canceled += DisableRayInteractor;
        rayInteractorActivate.action.Enable();
        teleportActivate.action.started += EnableTeleport;
        teleportActivate.action.canceled += DisableTeleport;
        teleportActivate.action.Enable();

        DisableRayInteractor(new InputAction.CallbackContext());
        DisableTeleport(new InputAction.CallbackContext());
    }

    private void EnableRayInteractor(InputAction.CallbackContext context) {
        SwitchToInteractor(directInteractor, rayInteractor);
    }

    private void DisableRayInteractor(InputAction.CallbackContext context) {
        SwitchToInteractor(rayInteractor, directInteractor);
    }

     private void EnableTeleport(InputAction.CallbackContext context) {
        SetInteractorEnabled(teleportInteractor, true);
    }

    private void DisableTeleport(InputAction.CallbackContext context) {
        // Manually pass the select event so that the player teleports when they release the teleport button.
        if (teleportInteractor.interactablesHovered.Count > 0)
        {
            teleportInteractor.StartManualInteraction(teleportInteractor.interactablesHovered[0].transform.GetComponent<IXRSelectInteractable>());
            teleportInteractor.EndManualInteraction();
        }
        SetInteractorEnabled(teleportInteractor, false);
        SetInteractorEnabled(directInteractor, true);
    }

    private void SetInteractorEnabled(XRBaseInteractor interactor, bool enabled) {
        interactor.allowSelect = enabled;
        interactor.allowHover = enabled;
        XRInteractorLineVisual lineVisual = interactor.gameObject.GetComponent<XRInteractorLineVisual>();

        if (lineVisual != null) {
            lineVisual.enabled = enabled;
            /// If the ray interactor has a reticle, then disable it as well.
            if (lineVisual.reticle)
            {
                lineVisual.reticle.SetActive(enabled);
            }
        }
    }

    private void SwitchToInteractor(XRBaseInteractor fromInteractor, XRBaseInteractor toInteractor)
    {
        SetInteractorEnabled(toInteractor, true);


        ///Make sure that if we are switching interactors then the interactor selections carry on as well.
        if (fromInteractor.isSelectActive)
        {
            foreach (IXRSelectInteractable interactable in fromInteractor.interactablesSelected.ToArray())
            {
                //Make sure that if the player is selecting a simple interactable, then that selection does not transfer over.
                //TODO: Make so that a simple interactable isn't selected after the player releases the grab button.
                if (!(interactable.transform.GetComponent<XRSimpleInteractable>())){
                    toInteractor.interactionManager.SelectEnter(toInteractor, interactable);
                }
            }
        }

        SetInteractorEnabled(fromInteractor, false);
    }

    private void OnDestroy() {
        rayInteractorActivate.action.started -= EnableRayInteractor;
        rayInteractorActivate.action.canceled -= DisableRayInteractor;
        teleportActivate.action.started -= EnableTeleport;
        teleportActivate.action.canceled -= DisableTeleport;
        teleportActivate.action.Disable();
        rayInteractorActivate.action.Disable();
    }
}
