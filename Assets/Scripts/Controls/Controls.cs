using Valve.VR;

public static class Controls {

    #region fields
    private static ControlType grab;
    private static ControlType grabInteract;

    private static ControlType teleport;

    public static ControlType DevEnv { get; internal set; } = ControlType.Grip;

    #endregion

    #region Getters
    public static ControlType Grab {
        get {

            if (grab == ControlType.NotAssigned) {
                Logger.Warning("Control " + grab + " has not been assigned");
            }

            return grab;
        }
    }

    public static ControlType GrabInteract {
        get {

            if (grabInteract == ControlType.NotAssigned) {
                Logger.Warning("Control " + grabInteract + " has not been assigned");
            }

            return grabInteract;
        }
    }

    public static ControlType Teleport {
        get {

            if (teleport == ControlType.NotAssigned) {
                Logger.Warning("Control " + teleport + " has not been assigned");
            }

            return teleport;
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
        teleport = ControlType.Menu;
    }
}
