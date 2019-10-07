using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region fields
    public bool IsGrabbed { get => Connector.IsGrabbed; }

    public SteamVR_Input_Sources HandType { get; private set; }

    private VRHandControls controls;

    public HandCollider coll { get; private set; }

    public Interactable Interactable { get; private set; }

    [SerializeField]
    private Hand other;
    public Hand Other { get => other; }

    public HandConnector Connector { get; private set; }
    #endregion

    private void Start() {
        HandType = GetComponent<VRHandControls>().handType;
        coll = transform.GetChild(0).GetComponent<HandCollider>();
        controls = GetComponent<VRHandControls>();
        Connector = new HandConnector(transform);

        Assert.IsNotNull(other, "Other hand was null");
    }

    private void Update() {
        UpdateControls();
    }

    private void UpdateControls() {

        if (VRInput.GetControlDown(HandType, Controls.Grab)) {
            InteractWithObject();
        }
        if (VRInput.GetControlUp(HandType, Controls.Grab)) {
            UninteractWithObject();
        }

        if (VRInput.GetControlDown(HandType, Controls.GrabInteract)) {
            GrabInteract();
        }
        if (VRInput.GetControlUp(HandType, Controls.GrabInteract)) {
            GrabUninteract();
        }
    }

    #region Interaction
    public void InteractWithObject() {
        if (IsGrabbed) {
            Connector.ReleaseItem(Interactable, 0);
            return;
        }

        Interactable = coll.GetGrab();
        if (Interactable == null) {
            return;
        }

        if (Interactable.Types.IsOn(InteractableType.Grabbable)) {

            if (Interactable.State == InteractState.LuerlockAttatch) {

                var pair = LuerlockAdapter.GrabbingLuerlock(Interactable.Rigidbody);

                // needs access to luerlock
                pair.Value.Connector.ConnectItem(Interactable, pair.Key);
            } else {
                Connector.ConnectItem(Interactable, 0);
            }

        } else if (Interactable.Types.IsOn(InteractableType.Interactable)) {
            Interactable.Interact(this);
        }
    }

    public void UninteractWithObject() {
        if (IsGrabbed) {
            if (VRControlSettings.HoldToGrab) {
                // Release();
                if (Interactable.State == InteractState.LuerlockAttatch) {

                    var pair = LuerlockAdapter.GrabbingLuerlock(Interactable.Rigidbody);

                    pair.Value.Connector.ReleaseItem(Interactable, pair.Key);
                } else {

                    Connector.ReleaseItem(Interactable, 0);
                }
            }
        } else if (Interactable != null) {
            Interactable.Uninteract(this);
            Interactable = null;
        }
    }

    public void GrabInteract() {
        if (!IsGrabbed) {
            return;
        }

        Interactable = coll.GetGrab();

        if (Interactable.Types.AreOn(InteractableType.Grabbable, InteractableType.Interactable)) {
            Interactable.Interact(this);
        }
    }

    public void GrabUninteract() {
        if (!IsGrabbed) {
            return;
        }

        Interactable = coll.GetGrab();

        if (Interactable.Types.AreOn(InteractableType.Grabbable, InteractableType.Interactable)) {
            Interactable.Uninteract(this);
        }
    }
    #endregion

    private void OnJointBreak(float breakForce) {
        Logger.Print("Joint force broken: " + breakForce);
        Connector.ReleaseItem(Interactable, 0);
    }

    public static Hand GrabbingHand(Rigidbody rb) {
        foreach (VRHandControls controls in VRInput.Hands) {
            if (rb == controls.Hand.Connector.GrabbedRigidbody) {
                return controls.Hand;
            }
        }

        return null;
    }
}
