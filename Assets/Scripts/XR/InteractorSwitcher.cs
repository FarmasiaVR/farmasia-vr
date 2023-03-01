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

    private XRInteractionManager interactionManager;




    private void Start() {
        rayInteractorActivate.action.started += EnableRayInteractor;
        rayInteractorActivate.action.canceled += DisableRayInteractor;
        rayInteractorActivate.action.Enable();
        teleportActivate.action.started += EnableTeleport;
        teleportActivate.action.canceled += DisableTeleport;
        teleportActivate.action.Enable();

        interactionManager = directInteractor.interactionManager;

        DisableRayInteractor(new InputAction.CallbackContext());
        DisableTeleport(new InputAction.CallbackContext());
    }

    private void EnableRayInteractor(InputAction.CallbackContext context) {
        SetInteractorEnabled(rayInteractor, true);
    }



    private void DisableRayInteractor(InputAction.CallbackContext context) {
        // If the player is hovering over an object while disabling the ray interactor, then make the player select the hovered object.
        if (rayInteractor.interactablesHovered.Count > 0)
        {
            if (directInteractor.interactablesSelected.Count > 0)
            {
                // Drop anything the player was holding at that point so that the player cannot select multiple objects.
                interactionManager.SelectExit(directInteractor, directInteractor.interactablesSelected[0]);
            }
            interactionManager.SelectEnter(directInteractor, rayInteractor.interactablesHovered[0].transform.GetComponent<IXRSelectInteractable>());
        }

        SetInteractorEnabled(rayInteractor, false);
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

    private void OnDestroy() {
        rayInteractorActivate.action.started -= EnableRayInteractor;
        rayInteractorActivate.action.canceled -= DisableRayInteractor;
        teleportActivate.action.started -= EnableTeleport;
        teleportActivate.action.canceled -= DisableTeleport;
        teleportActivate.action.Disable();
        rayInteractorActivate.action.Disable();
    }
}
