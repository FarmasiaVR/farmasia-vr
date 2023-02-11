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
    private GameObject rayInteractor;
    [SerializeField]
    private GameObject directInteractor;

    [Space]
    [Header("Input Events")]

    [SerializeField]
    private InputActionReference rayInteractorActivate;

    private void Start() {
        rayInteractorActivate.action.started += EnableRayInteractor;
        rayInteractorActivate.action.canceled += DisableRayInteractor;
        rayInteractorActivate.action.Enable();
    }

    private void EnableRayInteractor(InputAction.CallbackContext context) {
        Debug.Log("Enabling rays");
        directInteractor.SetActive(false);
        rayInteractor.SetActive(true);
    }

    private void DisableRayInteractor(InputAction.CallbackContext context) {
        rayInteractor.SetActive(false);
        directInteractor.SetActive(true);
        Debug.Log("Disabled ray interactor");
    }
}
