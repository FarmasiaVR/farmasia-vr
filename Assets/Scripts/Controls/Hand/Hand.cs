using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region Fields

    public GameObject placeholderPrefab;
    public bool IsInteracting { get => interactedInteractable != null; }
    public bool IsGrabbed { get => Connector.IsGrabbed; }
    public bool IsClean { get; set; }

    public HandSmoother Smooth { get; private set; }

    private bool IsTryingToGrab { get => !IsGrabbed && VRInput.GetControl(HandType, Controls.Grab); }
    private bool InRemoteGrabMode { get => !IsGrabbed && VRInput.GetControl(HandType, Controls.RemoteGrabMode); }

    private static float extendedGrabAngle = 30f;

    [SerializeField]
    private bool useHighlighting;

    public SteamVR_Input_Sources HandType { get; private set; }

    public HandCollider HandCollider { get; private set; }
    public HandCollider ExtendedHandCollider { get; private set; }

    public HandConnector Connector { get; private set; }
    public Interactable interactedInteractable;

    [SerializeField]
    private Hand other;
    public Hand Other { get => other; }

    private Transform offset;
    public Vector3 ColliderPosition { get => HandCollider.transform.position; }

    private RemoteGrabLine line;
    public bool RemoteGrabbing { get; private set; }
    #endregion

    private void Start() {
        HandCollider = transform.Find("HandColl").GetComponent<HandCollider>();
        ExtendedHandCollider = transform.Find("ExtendedHandColl").GetComponent<HandCollider>();
        HandType = GetComponent<VRActionsMapper>()?.handType ?? SteamVR_Input_Sources.Any;
        Connector = new HandConnector(this);

        Assert.IsFalse(HandType == SteamVR_Input_Sources.Any, "Invalid hand type");
        Assert.IsNotNull(HandCollider, "Missing HandCollider component");
        Assert.IsNotNull(other, "Other hand was null");
        offset = transform.Find("Offset");

        GameObject handSmooth = Instantiate(Resources.Load<GameObject>("Prefabs/HandSmoother"));
        Smooth = handSmooth.GetComponent<HandSmoother>();
        Smooth.Hand = this;

        line = transform.GetComponentInChildren<RemoteGrabLine>();
    }

    private void Update() {
        UpdateControls();
        UpdateHighlight();

        Interactable interactable = IsGrabbed ? Connector.GrabbedInteractable : interactedInteractable;
        interactable?.OnGrab(this);
    }

    private void UpdateControls() {

        // Grabbing
        if (VRInput.GetControlDown(HandType, Controls.Grab)) {
            Interact();
        }
        if (VRInput.GetControlUp(HandType, Controls.Grab)) {
            Uninteract();
        }

        // Interacting
        if (VRInput.GetControl(HandType, Controls.Grab)) {
            if (VRInput.GetControlDown(HandType, Controls.GrabInteract)) {
                GrabInteract();
            }
            if (VRInput.GetControlUp(HandType, Controls.GrabInteract)) {
                GrabUninteract();
            }
        } else {
            if (InRemoteGrabMode) {
                StartRemoteGrab();
            }
        }
    }



    private void UpdateHighlight() {
        if (!useHighlighting || IsGrabbed) {
            return;
        }

        if (!IsTryingToGrab) {
            HandCollider.HighlightClosestObject();
        }
    }

    private void StartRemoteGrab() {
        if (RemoteGrabbing) {
            return;
        }

        RemoteGrabbing = true;

        StartCoroutine(RemoteGrabCoroutine());
    }

    private IEnumerator RemoteGrabCoroutine() {
        while (RemoteGrabbing) {
            UpdateRemoteGrab();
            yield return null;
        }
        line.Enable(false);
    }

    private void UpdateRemoteGrab() {

        line.Enable(InRemoteGrabMode);
        if (InRemoteGrabMode) {
            Interactable pointedObj = ExtendedHandCollider.GetPointedObject(extendedGrabAngle);

            if (pointedObj != null && VRInput.GetControlDown(HandType, Controls.RemoteGrab)) {
                RemoteGrab(pointedObj);
            }

            ExtendedHandCollider.HighlightPointedObject(extendedGrabAngle);
        } else {
            ExtendedHandCollider.UnhighlightPrevious();
            RemoteGrabbing = false;
        }
    }


    #region Interaction
    public void Interact() {
        if (IsGrabbed) {
            return;
        }

        Interactable interactable = HandCollider.GetClosestInteractable();
        if (interactable == null) {
            // StartRemoteGrab();
        } else {
            InteractWith(interactable);
        }
    }

    public void InteractWith(Interactable interactable, bool setOffset = true) {
        if (setOffset) SetOffset(interactable.transform.position, interactable.transform.rotation);
        
        HandCollider.Enable(false);
        ExtendedHandCollider.Enable(false);
        interactable.Highlight.Unhighlight();

        if (interactable is AttachmentItem attachment && attachment.Attached) {
            interactable = HandleAttachedItem(attachment);
            Logger.Print($"Grabbed {interactable.name}");
        }

        if (interactable.Type == InteractableType.Grabbable) {
            Smooth.StartGrab();
            Connector.ConnectItem(interactable);
            interactedInteractable = interactable;
            interactedInteractable.OnGrabStart(this);
            Events.FireEvent(EventType.GrabObject, CallbackData.Object(this));
        } else if (interactable.Type == InteractableType.Interactable) {
            interactedInteractable = interactable;
            interactedInteractable.Interact(this);
            Events.FireEvent(EventType.InteractWithObject, CallbackData.Object(this));
        }
    }

    private AttachmentItem HandleAttachedItem(AttachmentItem attachment) {
        AttachmentItem parent = attachment.GetParent();

        if (parent is PumpFilterLid lid) {
            lid.MakeGrabbable(transform.position);
            return lid;
        }

        if (parent == other.interactedInteractable) {
            attachment.StartCoroutine(attachment.WaitForDistance(this));
            GameObject placeHolder = Instantiate(placeholderPrefab);
            return placeHolder.GetComponent<AttachmentItem>();
        } else {
            return attachment.GetParent();
        }
    }

    public void Uninteract() {

        HandCollider.Enable(true);
        ExtendedHandCollider.Enable(true);

        if (IsGrabbed) {
            Connector.Connection.Remove();
            interactedInteractable.OnGrabEnd(this);
            interactedInteractable = null;
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
        }
    }

    public void GrabUninteract() {
        if (CanGrabInteract()) {
            Connector.GrabbedInteractable.Uninteract(this);
            Events.FireEvent(EventType.GrabUninteractWithObject, CallbackData.Object(this));
        }
    }

    private bool CanGrabInteract() {
        return IsGrabbed && Connector.GrabbedInteractable.Type.AreOn(
            InteractableType.Grabbable, InteractableType.Interactable
        );
    }
    #endregion

    #region Remote grab
    private void RemoteGrab(Interactable i) {
        if (ItemIsPartOfGrabbedLuerlockSystem(i)) {
            return;
        }

        Interactable hitInt;
        if (!CanGrabObject(transform.position, i.transform.position, i, out hitInt)) {
            if (hitInt != null) {
                RemoteGrab(hitInt);
            }
            return;
        }

        if (i.Type == InteractableType.Grabbable) {
            MoveObject(i, transform.position);
        }

        GrabUninteract();
        Uninteract();
        InteractWith(i);

        RemoteGrabbing = false;
    }
    private bool ItemIsPartOfGrabbedLuerlockSystem(Interactable interactable) {
        if (interactable.State == InteractState.LuerlockAttached) {
            LuerlockAdapter luerlock = interactable.Interactors.LuerlockPair.Value;
            if (luerlock.State == InteractState.Grabbed) {
                return true;
            } else if (luerlock.GrabbedObjectCount > 0) {
                return true;
            }
        } else if (interactable as LuerlockAdapter is var l && l != null) {
            if (l.GrabbedObjectCount > 0) {
                return true;
            }
        }
        return false;
    }
    private void MoveObject(Interactable interactable, Vector3 position) {
        if (interactable.State == InteractState.LuerlockAttached) {
            Vector3 offset = position - interactable.transform.position;
            interactable.Interactors.LuerlockPair.Value.transform.position += offset;
        } else if (interactable.State == InteractState.ConnectableAttached) {
            Vector3 offset = position - interactable.transform.position;
            interactable.Interactors.ConnectableItem.transform.position += offset;
        } else {
            interactable.transform.position = position;
        }
    }
    private bool CanGrabObject(Vector3 pos, Vector3 targetPos, Interactable target, out Interactable hitInt) {

        RaycastHit hit;
        if (Physics.Raycast(pos, targetPos - pos, out hit, Vector3.Distance(pos, targetPos), int.MaxValue, QueryTriggerInteraction.Ignore)) {
            hitInt = Interactable.GetInteractable(hit.collider.transform);
            return hitInt == target;
        }

        hitInt = null;
        return false;
    }
    #endregion

    public static Hand GrabbingHand(Interactable interactable) {
        foreach (VRActionsMapper controls in VRInput.Hands) {
            if (interactable == controls.Hand.Connector.GrabbedInteractable) {
                return controls.Hand;
            } else if (interactable == controls.Hand.interactedInteractable) {
                return controls.Hand;
            }
        }

        return null;
    }

    public void SetOffset(Vector3 pos, Quaternion rot) {
        offset.position = pos;
        offset.rotation = rot;
        Smooth.transform.position = pos;
        Smooth.transform.rotation = rot;
    }
    public Transform GetOffset() {
        return offset;
    }
}
