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

    [Space]
    [Header("Input Events")]

    [SerializeField]
    private InputActionReference rayInteractorActivate;

    private void Start() {
        rayInteractorActivate.action.started += EnableRayInteractor;
        rayInteractorActivate.action.canceled += DisableRayInteractor;
        rayInteractorActivate.action.Enable();
        DisableRayInteractor(new InputAction.CallbackContext());
    }

    private void EnableRayInteractor(InputAction.CallbackContext context) {
        Debug.Log("Enabling rays");
        directInteractor.allowHover = false;
        directInteractor.allowSelect = false;
        rayInteractor.allowHover = true;
        rayInteractor.allowSelect = true;
        rayInteractor.gameObject.GetComponent<XRInteractorLineVisual>().enabled = true;
    }

    private void DisableRayInteractor(InputAction.CallbackContext context) {
        directInteractor.allowHover = true;
        directInteractor.allowSelect = true;
        rayInteractor.allowHover = false;
        rayInteractor.allowSelect = false;
        rayInteractor.gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
        Debug.Log("Disabled ray interactor");
    }
}
