using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region Fields
    public bool IsInteracting { get => interactedInteractable != null; }
    public bool IsGrabbed { get => Connector.IsGrabbed; }
    public bool IsClean { get; set; }

    public SteamVR_Input_Sources HandType { get; private set; }

    private HandCollider handCollider;

    public HandConnector Connector { get; private set; }
    private Interactable interactedInteractable;

    [SerializeField]
    private Hand other;
    public Hand Other { get => other; }

    public Transform Offset { get; private set; }
    public Vector3 ColliderPosition { get => handCollider.transform.position; }
    #endregion

    private void Start() {
        handCollider = GetComponentInChildren<HandCollider>();
        HandType = GetComponent<VRHandControls>()?.handType ?? SteamVR_Input_Sources.Any;
        Connector = new HandConnector(this);

        Assert.IsFalse(HandType == SteamVR_Input_Sources.Any, "Invalid hand type");
        Assert.IsNotNull(handCollider, "Missing HandCollider component");
        Assert.IsNotNull(other, "Other hand was null");
        Offset = transform.Find("Offset");
    }

    private void Update() {
        UpdateControls();

        Interactable interactable = IsGrabbed ? Connector.GrabbedInteractable : interactedInteractable;
        interactable?.UpdateInteract(this);
    }

    private void UpdateControls() {

        if (VRInput.GetControlDown(HandType, ControlType.Grip)) {
            HintBox.CreateHint("TEst hin");
        }

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

        Interactable interactable = handCollider.GetGrabbedInteractable();
        if (interactable == null) {
            Logger.Warning("No interactable to grab");
            return;
        }

        if (interactable.Type == InteractableType.Grabbable) {
            Offset.position = interactable.transform.position;
            Offset.rotation = interactable.transform.rotation;
            Connector.ConnectItem(interactable, 0);
            Events.FireEvent(EventType.GrabObject, CallbackData.Object(this));
        } else if (interactable.Type == InteractableType.Interactable) {
            interactedInteractable = interactable;
            interactedInteractable.Interact(this);
            Events.FireEvent(EventType.InteractWithObject, CallbackData.Object(this));
        }
    }

    public void ReleaseObject() {
        if (IsGrabbed) {
            Connector.ReleaseItem(0);
            Events.FireEvent(EventType.ReleaseObject, CallbackData.Object(this));
        } else if (interactedInteractable != null) {
            interactedInteractable.Uninteract(this);
            interactedInteractable = null;
            Events.FireEvent(EventType.UninteractWithObject, CallbackData.Object(this));
        }
    }

    public void GrabInteract() {
        if (CanGrabInteract()) {
            Connector.GrabbedInteractable.Interact(this);
            Events.FireEvent(EventType.GrabInteractWithObject, CallbackData.Object(this));
        } else {
            Logger.Error("GrabInteract(): Invalid state");
            ReleaseObject();
        }
    }

    public void GrabUninteract() {
        if (CanGrabInteract()) {
            Connector.GrabbedInteractable.Uninteract(this);
            Events.FireEvent(EventType.GrabUninteractWithObject, CallbackData.Object(this));
        } else {
            Logger.Error("GrabUninteract(): Invalid state");
            ReleaseObject();
        }
    }

    private bool CanGrabInteract() {
        return IsGrabbed && Connector.GrabbedInteractable.Type.AreOn(
            InteractableType.Grabbable, InteractableType.Interactable
        );
    }
    #endregion

    public static Hand GrabbingHand(Rigidbody rb) {
        foreach (VRHandControls controls in VRInput.Hands) {
            if (rb == controls.Hand.Connector.GrabbedInteractable?.GetComponent<Rigidbody>()) {
                return controls.Hand;
            }
        }

        return null;
    }
}
