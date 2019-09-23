using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

public static class VRInput {

    private static Dictionary<HandControl, ControlState> controls;

    private struct HandControl {
        Control Control;
        SteamVR_Input_Sources HandType;

        public HandControl(Control control, SteamVR_Input_Sources handType) {
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

    static VRInput() {
        controls = new Dictionary<HandControl, ControlState>();

        Control[] types = Enum.GetValues(typeof(Control)).Cast<Control>().ToArray();

        foreach (Control control in types) {
            controls.Add(new HandControl(control, SteamVR_Input_Sources.RightHand), new ControlState());
            controls.Add(new HandControl(control, SteamVR_Input_Sources.LeftHand), new ControlState());
        }
    }

    public static void SetControlState(Control c, SteamVR_Input_Sources handType, int getDown, bool down, int getUp) {

        HandControl hc = new HandControl(c, handType);

        ControlState state = controls[hc];
        state.GetDown = getUp;
        state.Down = down;
        state.GetUp = getUp;

        controls[hc] = state;
    }

    public static void ControlDown(Control c, SteamVR_Input_Sources handType) {

        HandControl hc = new HandControl(c, handType);

        ControlState state = controls[hc];
        state.GetDown = Time.frameCount;
        state.Down = true;

        controls[hc] = state;
    }
    public static void ControlUp(Control c, SteamVR_Input_Sources handType) {

        HandControl hc = new HandControl(c, handType);

        ControlState state = controls[hc];
        state.Down = false;
        state.GetUp = Time.frameCount;

        controls[hc] = state;
    }

    public static bool GetControlDown(Control c, SteamVR_Input_Sources handType) {
        HandControl hc = new HandControl(c, handType);
        return controls[hc].GetDown == Time.frameCount;
    }
    public static bool GetControl(Control c, SteamVR_Input_Sources handType) {
        HandControl hc = new HandControl(c, handType);
        return controls[hc].Down;
    }
    public static bool GetControlUp(Control c, SteamVR_Input_Sources handType) {
        HandControl hc = new HandControl(c, handType);
        return controls[hc].GetUp == Time.frameCount;
    }
}