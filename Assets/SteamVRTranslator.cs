using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using Valve.VR;



public class SteamVRTranslator : MonoBehaviour
{


    public SteamVR_Input_Sources inputSource;

    public InputActionReference TriggerButtonAction;
    public InputActionReference LaserButtonAction;
    public InputActionReference UseItemButtonAction;
    public InputActionReference MoveButtonAction;

    public GameObject legacyController;


    private void Start() {

        pairButtonAndCallBacks(TriggerButtonAction, grabActivate, grabDeActivate);

        pairButtonAndCallBacks(LaserButtonAction, LaserActivated, LaserDeActivated);

        pairButtonAndCallBacks(UseItemButtonAction, useItemActivated, useItemDeActivated);

        pairButtonAndCallBacks(MoveButtonAction, moveButttonActivated, moveButttonDeActivated);

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

        unPairButtonAndCallBacks(MoveButtonAction, moveButttonActivated, moveButttonDeActivated);
    }


    void Update() {

       

    }

    void grabActivate(InputAction.CallbackContext context) {
        Debug.Log("activated grab");
        VRInput.ControlDown(ControlType.TriggerClick, inputSource);
       // VRInput.ControlDown(ControlType.Grip, SteamVR_Input_Sources.RightHand);
    }

    void grabDeActivate(InputAction.CallbackContext context) {
        Debug.Log("Deactivated grab");
        VRInput.ControlUp(ControlType.TriggerClick, inputSource);
        //VRInput.ControlUp(ControlType.Grip, SteamVR_Input_Sources.RightHand);
    }

    void LaserActivated(InputAction.CallbackContext context) {

        Debug.Log("Laser activated");

        
       

        VRInput.ControlDown(ControlType.DPadNorth, inputSource);
    }


    void LaserDeActivated(InputAction.CallbackContext context) {

        Debug.Log("Laser de activated");



        VRInput.ControlUp(ControlType.DPadNorth, inputSource);
    }


    void useItemActivated(InputAction.CallbackContext context) {
        Debug.Log("Activated Item Use");
        VRInput.ControlDown(ControlType.PadClick, inputSource);
    }

    void useItemDeActivated(InputAction.CallbackContext context) {
        Debug.Log("De Activated Item Use");
        VRInput.ControlUp(ControlType.PadClick, inputSource);
    }



    void moveButttonActivated(InputAction.CallbackContext context) {
        Debug.Log("Move activated");
        VRInput.ControlDown(ControlType.Menu, inputSource);
    }

    void moveButttonDeActivated(InputAction.CallbackContext context) {
        Debug.Log("Move De activated");
        VRInput.ControlUp(ControlType.Menu, inputSource);
    }

}
