using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRHandControls : MonoBehaviour {

    public SteamVR_Action_Boolean grab;
    public SteamVR_Action_Boolean padClick;
    public SteamVR_Action_Skeleton skeleton;

    public SteamVR_Input_Sources handType;

    private Hand hand;

    void Start() {

        hand = GetComponent<Hand>();

        grab.AddOnStateDownListener(TriggerDown, handType);
        grab.AddOnStateUpListener(TriggerUp, handType);

        padClick.AddOnStateDownListener(PadDown, handType);
        padClick.AddOnStateUpListener(PadUp, handType);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        Debug.Log("Trigger is down: " + handType);

        hand.InteractWithObject();
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        Debug.Log("Trigger is up: " + handType);

        hand.UninteractWithObject();
    }
    
    public void PadDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {

        Debug.Log("Pad clicked");

        hand.GrabInteract();
    }
    public void PadUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        hand.GrabUninteract();
    }

    public SteamVR_Action_Skeleton Skeleton {
        get => skeleton;
    }
}
