using System;
using UnityEngine;
using Valve.VR;

public class VRHandControls : MonoBehaviour {

    #region fields
    [SerializeField]
    private SteamVR_Action_Boolean
        triggerTouch,
        triggerClick,
        menu,
        padTouch,
        padClick,
        grip;

    [SerializeField]
    private SteamVR_Action_Single triggerValue;

    [SerializeField]
    private SteamVR_Action_Vector2 padTouchValue;

    [SerializeField]
    private SteamVR_Action_Skeleton skeleton;

    public SteamVR_Action_Skeleton Skeleton {
        get => skeleton;
    }

    public SteamVR_Input_Sources handType;

    public Hand Hand { get; private set; }
    private TeleportControls teleport;
    #endregion

    private void Start() {

        if (NullChecks(triggerClick, triggerTouch, menu, padTouch, padClick, grip, triggerValue, padTouchValue, skeleton)) {
            throw new System.Exception("All controls are not initialized");
        }

        Hand = GetComponent<Hand>();
        teleport = GetComponent<TeleportControls>();

        VRInput.SetHandControls(handType, this);

        triggerTouch.AddOnStateDownListener(TriggerTouchDown, handType);
        triggerTouch.AddOnStateUpListener(TriggerTouchUp, handType);

        triggerClick.AddOnStateDownListener(TriggerDown, handType);
        triggerClick.AddOnStateUpListener(TriggerUp, handType);

        triggerValue.AddOnChangeListener(TriggerValueChange, handType);

        menu.AddOnStateDownListener(MenuDown, handType);
        menu.AddOnStateUpListener(MenuUp, handType);


        padTouch.AddOnStateDownListener(PadTouchDown, handType);
        padTouch.AddOnStateUpListener(PadTouchUp, handType);

        padClick.AddOnStateDownListener(PadDown, handType);
        padClick.AddOnStateUpListener(PadUp, handType);

        padTouchValue.AddOnChangeListener(PadTouchValueChange, handType);


        grip.AddOnStateDownListener(GripDown, handType);
        grip.AddOnStateUpListener(GripUp, handType);
    }

    

    private bool NullChecks(params SteamVR_Action[] actions) {
        foreach (var action in actions) {
            if (action == null) {
                return true;
            }
        }

        return false;
    }

    #region Trigger
    private void TriggerTouchDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(ControlType.TriggerTouch, handType);
    }

    private void TriggerTouchUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(ControlType.TriggerTouch, handType);
    }

    private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(ControlType.TriggerClick, handType);
    }

    private void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(ControlType.TriggerClick, handType);
    }

    private void TriggerValueChange(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float newAxis, float newDelta) {
        VRInput.SetTriggerValue(handType, newAxis, newDelta);
    }
    #endregion

    #region Menu
    private void MenuDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(ControlType.Menu, handType);
    }

    private void MenuUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(ControlType.Menu, handType);
    }
    #endregion

    #region Pad
    private void PadTouchDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(ControlType.PadTouch, handType);
    }

    private void PadTouchUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(ControlType.PadTouch, handType);
    }

    private void PadDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(ControlType.PadClick , handType);
    }

    private void PadUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(ControlType.PadClick, handType);
    }

    private void PadTouchValueChange(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta) {
        VRInput.SetPadTouchValue(handType, axis, delta);
    }
    #endregion

    #region Grip
    private void GripDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(ControlType.Grip, handType);
    }

    private void GripUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(ControlType.Grip, handType);
    }
    #endregion
}

