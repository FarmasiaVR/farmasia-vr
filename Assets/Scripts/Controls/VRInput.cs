using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

public static class VRInput {

    #region fields
    private static Dictionary<HandControl, ControlState> controls;

    public static VRActionsMapper[] Hands { get; private set; }
    private static Vector2[] padTouches;
    private static float[] triggerValues;

    private struct HandControl {
        ControlType Control;
        SteamVR_Input_Sources HandType;

        public HandControl(ControlType control, SteamVR_Input_Sources handType) {
            this.Control = control;
            this.HandType = handType;
        }

        public override bool Equals(object obj) {

            if (GetType() != obj.GetType()) {
                return false;
            }

            HandControl other = (HandControl)obj;

            return Control == other.Control && HandType == other.HandType;
        }

        public override int GetHashCode() {
            var hashCode = 1055286454;
            hashCode = hashCode * -1521134295 + Control.GetHashCode();
            hashCode = hashCode * -1521134295 + HandType.GetHashCode();
            return hashCode;
        }
    }
    #endregion

    #region Constructors
    static VRInput() {
        controls = new Dictionary<HandControl, ControlState>();

        ControlType[] types = Enum.GetValues(typeof(ControlType)).Cast<ControlType>().ToArray();

        foreach (ControlType control in types) {
            if (control == ControlType.NotAssigned) {
                continue;
            }
            controls.Add(new HandControl(control, SteamVR_Input_Sources.RightHand), new ControlState());
            controls.Add(new HandControl(control, SteamVR_Input_Sources.LeftHand), new ControlState());
        }

        Hands = new VRActionsMapper[2];
        padTouches = new Vector2[4];
        triggerValues = new float[4];
    }
    #endregion

    #region Controls
    private static void SetControlState(ControlType c, SteamVR_Input_Sources handType, int getDown, bool down, int getUp) {

        HandControl key = new HandControl(c, handType);

        ControlState state = controls[key];
        state.GetDown = getUp;
        state.Down = down;
        state.GetUp = getUp;

        controls[key] = state;
    }

    public static void ControlDown(ControlType c, SteamVR_Input_Sources handType) {

        Logger.Print("Down: " + c.ToString());
        HandControl key = new HandControl(c, handType);

        ControlState state = controls[key];
        state.GetDown = Time.frameCount;
        state.Down = true;
        controls[key] = state;
    }
    public static void ControlUp(ControlType c, SteamVR_Input_Sources handType) {

        HandControl key = new HandControl(c, handType);

        ControlState state = controls[key];
        state.Down = false;
        state.GetUp = Time.frameCount;
        controls[key] = state;
    }

    public static bool GetControlDown(SteamVR_Input_Sources handType, ControlType c) {

        HandControl key = new HandControl(c, handType);

    #if UNITY_NONVRCOMPUTER

        // Fix for TestHandMover

        int frameDifference = controls[key].GetDown - Time.frameCount;
        if (frameDifference == -1) {
            return controls[key].Down;
        }

        return controls[key].GetDown == Time.frameCount;
    #else 
        return controls[key].GetDown == Time.frameCount;
    #endif
    }

    public static bool GetControl(SteamVR_Input_Sources handType, ControlType c) {
        HandControl key = new HandControl(c, handType);
        return controls[key].Down;
    }
    public static bool GetControlUp(SteamVR_Input_Sources handType, ControlType c) {
        HandControl key = new HandControl(c, handType);

     #if UNITY_NONVRCOMPUTER

        // Fix for TestHandMover

        int frameDifference = controls[key].GetUp - Time.frameCount;
        if (frameDifference == -1) {
            return controls[key].GetUp > 10;
        }

        return controls[key].GetUp == Time.frameCount;
    #else
        return controls[key].GetUp == Time.frameCount;
    #endif
    }
    #endregion

    #region Skeletons
    public static void SetHandControls(SteamVR_Input_Sources handType, VRActionsMapper hand) {
        if (handType == SteamVR_Input_Sources.RightHand) {
            Hands[0] = hand;
        } else if (handType == SteamVR_Input_Sources.LeftHand) {
            Hands[1] = hand;
        }
    }
    public static SteamVR_Action_Skeleton Skeleton(SteamVR_Input_Sources handType) {
        if (handType == SteamVR_Input_Sources.RightHand) {
            return RightSkeleton;
        } else if (handType == SteamVR_Input_Sources.LeftHand) {
            return LeftSkeleton;
        }

        return null;
    }

    public static SteamVR_Action_Skeleton RightSkeleton {
        get {
            return Hands[0].Skeleton;
        }
    }
    public static SteamVR_Action_Skeleton LeftSkeleton {
        get {
            return Hands[1].Skeleton;
        }
    }
    #endregion

    #region Pad touch value
    public static void SetPadTouchValue(SteamVR_Input_Sources handType, Vector2 value, Vector2 delta) {
        if (handType == SteamVR_Input_Sources.RightHand) {
            RightPadValue = value;
            RightPadDelta = delta;
        } else if (handType == SteamVR_Input_Sources.LeftHand) {
            LeftPadValue = value;
            LeftPadDelta = delta;
        }
    }

    public static Vector2 PadTouchValue(SteamVR_Input_Sources handType) {
        if (handType == SteamVR_Input_Sources.RightHand) {
            return RightPadValue;
        } else if (handType == SteamVR_Input_Sources.LeftHand) {
            return LeftPadValue;
        }

        return Vector2.zero;
    }

    public static Vector2 PadTouchDelta(SteamVR_Input_Sources handType) {
        if (handType == SteamVR_Input_Sources.RightHand) {
            return RightPadDelta;
        } else if (handType == SteamVR_Input_Sources.LeftHand) {
            return LeftPadDelta;
        }

        return Vector2.zero;
    }

    public static Vector2 RightPadValue {
        get {
            return padTouches[0];
        }
        private set {
            padTouches[0] = value;
        }
    }
    public static Vector2 RightPadDelta {
        get {
            return padTouches[1];
        }
        private set {
            padTouches[1] = value;
        }
    }
    public static Vector2 LeftPadValue {
        get {
            return padTouches[2];
        }
        private set {
            padTouches[2] = value;
        }
    }
    public static Vector2 LeftPadDelta {
        get {
            return padTouches[3];
        }
        private set {
            padTouches[3] = value;
        }
    }
    #endregion

    #region Trigger value
    public static void SetTriggerValue(SteamVR_Input_Sources handType, float value, float delta) {
        if (handType == SteamVR_Input_Sources.RightHand) {
            RightTriggerValue = value;
        } else if (handType == SteamVR_Input_Sources.LeftHand) {
            LeftTriggerValue = value;
        }
    }

    public static float TriggerValue(SteamVR_Input_Sources handType) {
        if (handType == SteamVR_Input_Sources.RightHand) {
            return RightTriggerValue;
        } else if (handType == SteamVR_Input_Sources.LeftHand) {
            return LeftTriggerValue;
        }

        return 0;
    }

    public static float RightTriggerValue {
        get {
            return triggerValues[0];
        }
        private set {
            triggerValues[0] = value;
        }
    }
    public static float RightTriggerDelta {
        get {
            return triggerValues[1];
        }
        private set {
            triggerValues[1] = value;
        }
    }
    public static float LeftTriggerValue {
        get {
            return triggerValues[2];
        }
        private set {
            triggerValues[2] = value;
        }
    }
    public static float LeftTriggerDelta {
        get {
            return triggerValues[3];
        }
        private set {
            triggerValues[3] = value;
        }
    }
    #endregion
}