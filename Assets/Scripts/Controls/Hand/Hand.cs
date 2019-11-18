using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region Fields
    public bool IsInteracting { get => interactedInteractable != null; }
    public bool IsGrabbed { get => Connector.IsGrabbed; }
    public bool IsClean { get; set; }
    private bool IsRemoteGrabbing { get => !IsGrabbed && VRInput.GetControl(HandType, Controls.Grab); }

    private static float extendedGrabTime = 1.5f;
    private static float extendedGrabAngle = 30f;

    [SerializeField]
    private bool useHighlighting;

    public SteamVR_Input_Sources HandType { get; private set; }

    private HandCollider handCollider;
    private HandCollider extendedHandCollider;
    private Pipeline remoteGrabPipe;
    private GameObject prevPointedObj;

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
        UpdateHighlight();
        UpdateRemoteGrab();

        Interactable interactable = IsGrabbed ? Connector.GrabbedInteractable : interactedInteractable;
        interactable?.Interacting(this);
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
        // This breaks toggle grab but that might not be a problem if we dont use it
        if (VRInput.GetControl(HandType, Controls.Grab)) {
            if (VRInput.GetControlDown(HandType, Controls.GrabInteract)) {
                GrabInteract();
            }
            if (VRInput.GetControlUp(HandType, Controls.GrabInteract)) {
                GrabUninteract();
            }
        }
    }

    private void UpdateHighlight() {
        if (!useHighlighting || IsGrabbed) {
            return;
        }

        if (IsRemoteGrabbing) {
            extendedHandCollider.HighlightPointedObject(extendedGrabAngle);
        } else {
            extendedHandCollider.UnhighlightPrevious();
            handCollider.HighlightClosestObject();
        }
    }

    private void UpdateRemoteGrab() {
        if (IsRemoteGrabbing) {
            GameObject pointedObj = extendedHandCollider.GetPointedObject(extendedGrabAngle);
            if (pointedObj != prevPointedObj) {
                remoteGrabPipe?.Abort();
                if (pointedObj != null) {
                    remoteGrabPipe = G.Instance.Pipeline
                                    .New()
                                    .Delay(extendedGrabTime)
                                    .TFunc(RemoteGrab, () => pointedObj);
                }
                prevPointedObj = pointedObj;
            }
        } else {
            prevPointedObj = null;
            remoteGrabPipe?.Abort();
            remoteGrabPipe = null;
        }
    }

    #region Interaction
    public void Interact() {
        if (IsGrabbed) {
            return;
        }

        Interactable interactable = handCollider.GetClosestInteractable();
        if (interactable != null) {
            InteractWith(interactable);
        } else {
            Logger.Print("No interactable to interact with");
        }
    }
    public void InteractWith(Interactable interactable) {

        Offset.position = interactable.transform.position;
        Offset.rotation = interactable.transform.rotation;

        if (interactable.Type == InteractableType.Grabbable) {
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
    private void RemoteGrab(GameObject obj) {
        Interactable i = obj;

        if (ItemIsPartOfGrabbedLuerlockSystem(i)) {
            return;
        }

        if (!CanGrabObject(transform.position, obj.transform.position, i)) {
            return; 
        }

        if (i.Type == InteractableType.Grabbable) {
            MoveObject(i, transform.position);
        }

        GrabUninteract();
        Uninteract();
        InteractWith(obj);
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
        } else {
            interactable.transform.position = position;
        }
    }
    private bool CanGrabObject(Vector3 pos, Vector3 targetPos, Interactable target) {

        RaycastHit hit;
        if (Physics.Raycast(pos, targetPos - pos, out hit, Vector3.Distance(pos, targetPos), int.MaxValue, QueryTriggerInteraction.Ignore)) {
            Logger.PrintVariables("Raycast hit", hit.collider.name);
            return Interactable.GetInteractable(hit.collider.transform) == target;
        }

        Logger.Print("Raycast did not hit");
        return false;
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
