using UnityEngine;
using Valve.VR;

public class VRHandControls : MonoBehaviour {

    #region fields
    public SteamVR_Action_Boolean grab;
    public SteamVR_Action_Boolean menuClick;
    public SteamVR_Action_Boolean padClick;
    public SteamVR_Action_Boolean grip;

    public SteamVR_Action_Skeleton skeleton;

    public SteamVR_Action_Skeleton Skeleton {
        get => skeleton;
    }

    public SteamVR_Input_Sources handType;

    private Hand hand;
    private TeleportControls teleport;
    #endregion

    private void Start() {
        hand = GetComponent<Hand>();
        teleport = GetComponent<TeleportControls>();

        grab.AddOnStateDownListener(TriggerDown, handType);
        grab.AddOnStateUpListener(TriggerUp, handType);

        menuClick.AddOnStateDownListener(MenuDown, handType);
        menuClick.AddOnStateUpListener(MenuUp, handType);

        padClick.AddOnStateDownListener(PadDown, handType);
        padClick.AddOnStateUpListener(PadUp, handType);

        grip.AddOnStateDownListener(GripDown, handType);
        grip.AddOnStateUpListener(GripUp, handType);
    }

    #region Trigger
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(Control.TriggerClick, handType);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(Control.TriggerClick, handType);
    }
    #endregion

    #region Menu
    public void MenuDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(Control.Menu, handType);
    }

    public void MenuUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(Control.Menu, handType);
    }
    #endregion

    #region Pad
    public void PadDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(Control.PadClick , handType);
    }

    public void PadUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(Control.PadClick, handType);
    }
    #endregion

    #region Grip
    public void GripDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlDown(Control.Grip, handType);
    }

    public void GripUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        VRInput.ControlUp(Control.Grip, handType);
    }
    #endregion
}

