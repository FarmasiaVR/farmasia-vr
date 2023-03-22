#if UNITY_ANDROID
using SteamVRMock;
#else
using Valve.VR;
#endif

public static class Controls {

#region Fields
    private static ControlType grab;
    private static ControlType grabInteract;

    private static ControlType remoteGrabMode;
    private static ControlType remoteGrab;

    private static ControlType takeMedicine;
    private static ControlType ejectMedicine;

    private static ControlType menu;

    public static ControlType DevEnv { get; internal set; } = ControlType.Grip;
#endregion

#region Getters
    public static ControlType Grab {
        get {

            if (grab == ControlType.NotAssigned) {
                Logger.Warning("Control grab has not been assigned");
            }

            return grab;
        }
    }

    public static ControlType GrabInteract {
        get {

            if (grabInteract == ControlType.NotAssigned) {
                Logger.Warning("Control grabInteract has not been assigned");
            }

            return grabInteract;
        }
    }

    public static ControlType RemoteGrabMode {
        get {
            if (remoteGrabMode == ControlType.NotAssigned) {
                Logger.Warning("Control remoteGrabMode has not been assigned");
            }

            return remoteGrabMode;
        }
    }

    public static ControlType RemoteGrab {
        get {

            if (remoteGrab == ControlType.NotAssigned) {
                Logger.Warning("Control remoteGrab has not been assigned");
            }

            return remoteGrab;
        }
    }

    public static ControlType TakeMedicine {
        get {

            if (takeMedicine == ControlType.NotAssigned) {
                Logger.Warning("Control takeMedicine has not been assigned");
            }

            return takeMedicine;
        }
    }

    public static ControlType EjectMedicine {
        get {

            if (ejectMedicine == ControlType.NotAssigned) {
                Logger.Warning("Control ejectMedicine has not been assigned");
            }

            return ejectMedicine;
        }
    }

    public static ControlType Menu {
        get {

            if (menu == ControlType.NotAssigned) {
                Logger.Warning("Control teleport has not been assigned");
            }

            return menu;
        }
    }

#endregion

#region Constructors
    static Controls() {
        SetDefaultControls();
    }
#endregion

    public static void SetDefaultControls() {
        grab = ControlType.TriggerClick;
        grabInteract = ControlType.PadClick;

        remoteGrabMode = ControlType.DPadNorth;
        remoteGrab = ControlType.TriggerClick;

        takeMedicine = ControlType.DPadWest;
        ejectMedicine = ControlType.DPadEast;

        menu = ControlType.Menu;
    }
}
