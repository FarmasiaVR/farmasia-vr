using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using Valve.VR;



public class SteamVRTranslator : MonoBehaviour
{


    public InputActionReference TriggerButtonAction;
    public InputActionReference LaserButtonAction;
    public InputActionReference UseItemButtonAction;
 
    public GameObject legacyController;


    private void Start() {

        pairButtonAndCallBacks(TriggerButtonAction, grabActivate, grabDeActivate);

        pairButtonAndCallBacks(LaserButtonAction, LaserActivated, LaserDeActivated);

        pairButtonAndCallBacks(UseItemButtonAction, useItemActivated, useItemDeActivated);

    }

    private void pairButtonAndCallBacks(InputActionReference refToPair, Action<InputAction.CallbackContext> activateFunc, Action<InputAction.CallbackContext> deActivateFunc) {
        refToPair.action.started += activateFunc;
        refToPair.action.canceled += deActivateFunc;
    }

    private void unPairButtonAndCallBacks(InputActionReference refToPair, Action<InputAction.CallbackContext> activateFunc, Action<InputAction.CallbackContext> deActivateFunc) {
        refToPair.action.started -= activateFunc;
        refToPair.action.canceled -= deActivateFunc;
    }


    private void OnDestroy() {

        unPairButtonAndCallBacks(TriggerButtonAction, grabActivate, grabDeActivate);

        unPairButtonAndCallBacks(LaserButtonAction, LaserActivated, LaserDeActivated);

        unPairButtonAndCallBacks(UseItemButtonAction, useItemActivated, useItemDeActivated);

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


    void useItemActivated(InputAction.CallbackContext context) {
        Debug.Log("Activated Item Use");
        VRInput.ControlDown(ControlType.PadClick, SteamVR_Input_Sources.RightHand);
    }

    void useItemDeActivated(InputAction.CallbackContext context) {
        Debug.Log("De Activated Item Use");
        VRInput.ControlUp(ControlType.PadClick, SteamVR_Input_Sources.RightHand);
    }

}
