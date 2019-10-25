using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region Fields
    public bool IsGrabbed { get => Connector.IsGrabbed; }
    public bool IsClean { get; set; }

    public SteamVR_Input_Sources HandType { get; private set; }

    private HandCollider handCollider;

    public HandConnector Connector { get; private set; }
    public Interactable GrabbedInteractable { get; private set; }

    [SerializeField]
    private Hand other;
    public Hand Other { get => other; }

    public Transform Offset { get; private set; }
    public Vector3 ColliderPosition { get => handCollider.transform.position; }
    #endregion

    private void Start() {
        handCollider = GetComponentInChildren<HandCollider>();
        HandType = GetComponent<VRHandControls>()?.handType ?? SteamVR_Input_Sources.Any;
        Connector = new HandConnector(transform);

        Assert.IsFalse(HandType == SteamVR_Input_Sources.Any, "Invalid hand type");
        Assert.IsNotNull(handCollider, "Missing HandCollider component");
        Assert.IsNotNull(other, "Other hand was null");
        Offset = transform.Find("Offset");
    }

    private void Update() {
        UpdateControls();

        if (IsGrabbed && GrabbedInteractable != null) {
            GrabbedInteractable.UpdateInteract(this);
        }
    }

    private void UpdateControls() {
        // Grabbing
        if (VRInput.GetControlDown(HandType, Controls.Grab)) {
            GrabObject();
        }
        if (VRInput.GetControlUp(HandType, Controls.Grab)) {
            ReleaseObject();
        }

        // Interacting
        if (VRInput.GetControlDown(HandType, Controls.GrabInteract)) {
            GrabInteract();
        }
        if (VRInput.GetControlUp(HandType, Controls.GrabInteract)) {
            GrabUninteract();
        }
    }


    #region Interaction
    public void GrabObject() {
        if (IsGrabbed) {
            return;
        }

        GrabbedInteractable = handCollider.GetGrab();
        if (GrabbedInteractable == null) {
            return;
        }

        Events.FireEvent(EventType.InteractWithObject, CallbackData.Object(this));
        if (GrabbedInteractable.Type == InteractableType.Grabbable) {
            Offset.position = GrabbedInteractable.transform.position;
            Offset.rotation = GrabbedInteractable.transform.rotation;
            Connector.ConnectItem(GrabbedInteractable, 0);
        } else if (GrabbedInteractable.Type == InteractableType.Interactable) {
            GrabbedInteractable.Interact(this);
        }
    }

    public void ReleaseObject() {
        if (IsGrabbed) {
            Events.FireEvent(EventType.UninteractWithObject, CallbackData.Object(this));
            Connector.ReleaseItem(0);
        } else if (GrabbedInteractable != null) {
            GrabbedInteractable.Uninteract(this);
        }
        GrabbedInteractable = null;
    }

    public void GrabInteract() {
        if (CanGrabInteract()) {
            Events.FireEvent(EventType.GrabInteractWithObject, CallbackData.Object(this));
            GrabbedInteractable.Interact(this);
        } else {
            Logger.Error("GrabInteract(): Invalid state");
            ReleaseObject();
        }
    }

    public void GrabUninteract() {
        if (CanGrabInteract()) {
            GrabbedInteractable.Uninteract(this);
        } else {
            Logger.Error("GrabUninteract(): Invalid state");
            ReleaseObject();
        }
    }

    private bool CanGrabInteract() {
        if (!IsGrabbed || GrabbedInteractable == null) {
            return false;
        }

        return GrabbedInteractable.Type.AreOn(InteractableType.Grabbable, InteractableType.Interactable);
    }
    #endregion

    private void OnJointBreak(float breakForce) {
        Logger.Print("Joint force broken: " + breakForce);
        Connector.ReleaseItem(0);
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
