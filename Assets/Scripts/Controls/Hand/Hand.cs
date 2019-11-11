using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region Fields
    public bool IsInteracting { get => interactedInteractable != null; }
    public bool IsGrabbed { get => Connector.IsGrabbed; }
    public bool IsClean { get; set; }

    private static float extendedGrabTime = 1.5f;
    private static float extendedGrabAngle = 30f;

    public SteamVR_Input_Sources HandType { get; private set; }

    private HandCollider handCollider;
    private HandCollider extendedHandCollider;

    public HandConnector Connector { get; private set; }
    private Interactable interactedInteractable;

    [SerializeField]
    private Hand other;
    public Hand Other { get => other; }

    public Transform Offset { get; private set; }
    public Vector3 ColliderPosition { get => handCollider.transform.position; }
    #endregion

    private void Start() {
        handCollider = transform.Find("HandColl").GetComponent<HandCollider>();
        extendedHandCollider = transform.Find("ExtendedHandColl").GetComponent<HandCollider>();
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
            Interact();
        }
        if (VRInput.GetControlUp(HandType, Controls.Grab)) {
            Uninteract();
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
    public void Interact() {
        if (IsGrabbed) {
            return;
        }

        Interactable interactable = handCollider.GetClosestInteractable();
        if (interactable == null) {
            Logger.Warning("No interactable to grab");
            RemoteGrab();
            return;
        }

        InteractWith(interactable);
    }
    private void InteractWith(Interactable interactable) {
        if (interactable.Type == InteractableType.Grabbable) {
            Offset.position = interactable.transform.position;
            Offset.rotation = interactable.transform.rotation;
            Connector.ConnectItem(interactable);
            Events.FireEvent(EventType.GrabObject, CallbackData.Object(this));
        } else if (interactable.Type == InteractableType.Interactable) {
            interactedInteractable = interactable;
            interactedInteractable.Interact(this);
            Events.FireEvent(EventType.InteractWithObject, CallbackData.Object(this));
        }
    }

    public void Uninteract() {
        if (IsGrabbed) {
            Connector.Connection.Remove();
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
            Uninteract();
        }
    }

    public void GrabUninteract() {
        if (CanGrabInteract()) {
            Connector.GrabbedInteractable.Uninteract(this);
            Events.FireEvent(EventType.GrabUninteractWithObject, CallbackData.Object(this));
        } else {
            Logger.Error("GrabUninteract(): Invalid state");
            Uninteract();
        }
    }

    private bool CanGrabInteract() {
        return IsGrabbed && Connector.GrabbedInteractable.Type.AreOn(
            InteractableType.Grabbable, InteractableType.Interactable
        );
    }
    #endregion

    #region Extended grab
    private void RemoteGrab() {
        StartCoroutine(RemoteGrabCoroutine());
    }

    private IEnumerator RemoteGrabCoroutine() {
        while (true) {

            if (extendedHandCollider.CountWithinAngle(extendedGrabAngle) > 0) {

                GameObject original = extendedHandCollider.GetPointedObject(extendedGrabAngle);
                float time = extendedGrabTime;

                while (time > 0) {

                    if (VRInput.GetControlUp(HandType, Controls.Grab)) {
                        break;
                    }

                    time -= Time.deltaTime;

                    GameObject current = extendedHandCollider.GetPointedObject(extendedGrabAngle);

                    if (current != original) {
                        break;
                    }

                    yield return null;
                }

                if (time <= 0) {
                    Interactable i = original;
                    if (i.Type == InteractableType.Grabbable) {
                        original.transform.position = transform.position;
                    }
                    InteractWith(original);
                    yield break;
                }
            }

            if (VRInput.GetControlUp(HandType, Controls.Grab)) {
                break;
            }

            yield return null;
        }
    }
    #endregion

    public static Hand GrabbingHand(Interactable interactable) {
        foreach (VRHandControls controls in VRInput.Hands) {
            if (interactable == controls.Hand.Connector.GrabbedInteractable) {
                return controls.Hand;
            }
        }

        return null;
    }
}
