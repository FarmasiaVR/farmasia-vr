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
    private bool interactorSwitchingEnabled;




    private void Start() {
        rayInteractorActivate.action.started += EnableRayInteractor;
        rayInteractorActivate.action.canceled += DisableRayInteractor;
        rayInteractorActivate.action.Enable();
        teleportActivate.action.started += EnableTeleport;
        teleportActivate.action.canceled += DisableTeleport;
        teleportActivate.action.Enable();

        interactorSwitchingEnabled = true;

        interactionManager = directInteractor.interactionManager;

        DisableRayInteractor(new InputAction.CallbackContext());
        DisableTeleport(new InputAction.CallbackContext());
    }

    public void EnableInteractorSwitching()
    {
        SetInteractorEnabled(directInteractor, true);
        interactorSwitchingEnabled = true;
        
    }

    private void EnableRayInteractor(InputAction.CallbackContext context) {
        // If the player is not allowed to interact with objects (for example, during a fade), then do not allow enabling of interactors.
        // This function is called through Input events even if this component is disabled.
        if (!interactorSwitchingEnabled) return;

        SetInteractorEnabled(rayInteractor, true);
    }



    private void DisableRayInteractor(InputAction.CallbackContext context) {
        if (!interactorSwitchingEnabled) return;

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
        if (!interactorSwitchingEnabled) return;

        SetInteractorEnabled(teleportInteractor, true);
    }

    private void DisableTeleport(InputAction.CallbackContext context) {
        if (!interactorSwitchingEnabled) return;

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
        if (!interactorSwitchingEnabled) return;

        interactor.allowHover = enabled;
        interactor.allowSelect = enabled;
        XRInteractorLineVisual lineVisual = interactor.gameObject.GetComponent<XRInteractorLineVisual>();

        if (lineVisual != null) {
            lineVisual.enabled = enabled;
            /// If the ray interactor has a reticle, then disable it as well.
            if (lineVisual.reticle && enabled==false)
            {
                // Disable the line reticle when disabling the interactor. Otherwise the reticle may remain in the scene.
                // The interactor should take care of re-enabling the reticle.
                lineVisual.reticle.SetActive(enabled);
            }
        }
    }

    public void DisableInteractorSwitching()
    {
        XRInteractionManager interactionManager = directInteractor.interactionManager;

        // If the player is holding something, then drop it. Otherwise the object will stay selected and may cause bugs.
        if (directInteractor.interactablesSelected.Count > 0)
        {
            interactionManager.SelectExit(directInteractor, directInteractor.interactablesSelected[0]);
        }

        SetInteractorEnabled(directInteractor, false);
        SetInteractorEnabled(rayInteractor, false);
        SetInteractorEnabled(teleportInteractor, false);
        interactorSwitchingEnabled = false;
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
