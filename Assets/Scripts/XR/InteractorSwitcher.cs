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
        Debug.Log("Enabling rays");
        SetInteractorEnabled(rayInteractor, true);
        SetInteractorEnabled(directInteractor, false);
    }

    private void DisableRayInteractor(InputAction.CallbackContext context) {
        SetInteractorEnabled(directInteractor, true);
        SetInteractorEnabled(rayInteractor, false);
        Debug.Log("Disabled ray interactor");
    }

     private void EnableTeleport(InputAction.CallbackContext context) {
        SetInteractorEnabled(teleportInteractor, true);
        SetInteractorEnabled(directInteractor, false);
    }

    private void DisableTeleport(InputAction.CallbackContext context) {
        // Manually pass the select event so that the player teleports when they release the teleport button.
        teleportInteractor.interactablesHovered[0].transform.GetComponent<BaseTeleportationInteractable>().teleportationProvider.QueueTeleportRequest(new TeleportRequest());
        SetInteractorEnabled(directInteractor, true);
        SetInteractorEnabled(teleportInteractor, false);
    }

    private void SetInteractorEnabled(XRBaseInteractor interactor, bool enabled) {
        interactor.allowSelect = enabled;
        interactor.allowHover = enabled;
        XRInteractorLineVisual lineVisual = interactor.gameObject.GetComponent<XRInteractorLineVisual>();
        if (lineVisual) {
            lineVisual.enabled = enabled;
        }
    }
}
