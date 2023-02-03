using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using Valve.VR;



public class testintestintesting : MonoBehaviour
{


    public InputActionReference TriggerButtonAction;
    public InputActionReference LaserButtonAction;

    public GameObject legacyController;


    private void Start() {
        TriggerButtonAction.action.started += grabActivate;
        TriggerButtonAction.action.canceled += grabDeActivate;

        LaserButtonAction.action.started += LaserActivated;
        LaserButtonAction.action.canceled += LaserDeActivated;
    }

    private void OnDestroy() {
        TriggerButtonAction.action.started -= grabActivate;
        TriggerButtonAction.action.canceled -= grabDeActivate;


        LaserButtonAction.action.started -= LaserActivated;
        LaserButtonAction.action.canceled -= LaserDeActivated;
    }


    void Update() {

       

    }

    void grabActivate(InputAction.CallbackContext context) {
        Debug.Log("activated grab");
        VRInput.ControlDown(ControlType.TriggerClick, SteamVR_Input_Sources.RightHand);
       // VRInput.ControlDown(ControlType.Grip, SteamVR_Input_Sources.RightHand);
    }

    void grabDeActivate(InputAction.CallbackContext context) {
        Debug.Log("Deactivated grab");
        VRInput.ControlUp(ControlType.TriggerClick, SteamVR_Input_Sources.RightHand);
        //VRInput.ControlUp(ControlType.Grip, SteamVR_Input_Sources.RightHand);
    }

    void LaserActivated(InputAction.CallbackContext context) {

        Debug.Log("Laser activated");

        
       

        VRInput.ControlDown(ControlType.DPadNorth, SteamVR_Input_Sources.RightHand);
    }


    void LaserDeActivated(InputAction.CallbackContext context) {

        Debug.Log("Laser de activated");



        VRInput.ControlUp(ControlType.DPadNorth, SteamVR_Input_Sources.RightHand);
    }
}
