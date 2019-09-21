using UnityEngine;
using Valve.VR;

public class VRHandControls : MonoBehaviour {

    #region fields
    public SteamVR_Action_Boolean grab;
    public SteamVR_Action_Boolean menuClick;
    public SteamVR_Action_Boolean padClick;

    public SteamVR_Action_Skeleton skeleton;

    public SteamVR_Action_Skeleton Skeleton {
        get => skeleton;
    }

    public SteamVR_Input_Sources handType;

    private Hand hand;
    private TeleportControls teleport;
    #endregion

    void Start() {
        hand = GetComponent<Hand>();
        teleport = GetComponent<TeleportControls>();

        grab.AddOnStateDownListener(TriggerDown, handType);
        grab.AddOnStateUpListener(TriggerUp, handType);

        menuClick.AddOnStateDownListener(MenuDown, handType);
        menuClick.AddOnStateUpListener(MenuUp, handType);

        padClick.AddOnStateDownListener(PadDown, handType);
        padClick.AddOnStateUpListener(PadUp, handType);
    }

    #region Trigger
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        Logger.Print("Trigger is down: " + handType);

        hand.InteractWithObject();
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        Logger.Print("Trigger is up: " + handType);
        hand.UninteractWithObject();
    }
    #endregion

    #region Menu
    public void MenuDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        teleport.StartTeleport();
    }

    public void MenuUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        teleport.EndTeleport();
    }
    #endregion

    #region Pad
    public void PadDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        hand.GrabInteract();
    }

    public void PadUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        hand.GrabUninteract();
    }
    #endregion
}
