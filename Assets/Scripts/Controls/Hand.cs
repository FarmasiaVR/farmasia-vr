using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region fields
    public bool IsGrabbed { get; set; }

    private SteamVR_Input_Sources handType;

    private VRHandControls controls;
    private FixedJoint joint;
    private FixedJoint Joint {
        get {
            if (joint == null) {
                AddJoint();
            }
            return joint;
        }
    }

    private Vector3 velocity;
    private Vector3 lastPos;

    private Vector3 angularVelocity;
    private Vector3 lastEulerAngles;

    public HandCollider coll { get; private set; }

    public Interactable Interactable { get; private set; }
    public Rigidbody GrabbedRigidbody { get; private set; }
    private Vector3 grabOffset;
    private Vector3 rotOffset;

    [SerializeField]
    private Hand other;
    #endregion

    private void Start() {
        handType = GetComponent<VRHandControls>().handType;
        coll = transform.GetChild(0).GetComponent<HandCollider>();
        controls = GetComponent<VRHandControls>();
    }

    private void Update() {
        UpdateControls();
        UpdateVelocity();

        if (IsGrabbed) {
            UpdateGrabbedObject();
        }
    }

    private void UpdateControls() {

        if (VRInput.GetControlDown(Control.TriggerClick, handType)) {
            InteractWithObject();
        }
        if (VRInput.GetControlUp(Control.TriggerClick, handType)) {
            UninteractWithObject();
        }

        if (VRInput.GetControlDown(Control.PadClick, handType)) {
            GrabInteract();
        }
        if (VRInput.GetControlUp(Control.PadClick, handType)) {
            GrabUninteract();
        }
    }
    // Alternative: set Rigidbody to kinematic, might cause bugs though
    private void UpdateGrabbedObject() {
        GrabbedRigidbody.velocity = Vector3.zero;
        // GrabbedRigidbody.transform.position = ColliderPosition + grabOffset;

        GrabbedRigidbody.angularVelocity = Vector3.zero;
        // GrabbedRigidbody.transform.eulerAngles = ColliderEulerAngles + rotOffset;
    }


    private void UpdateVelocity() {
        Vector3 diff = transform.position - lastPos;
        velocity = diff / Time.deltaTime;
        lastPos = transform.position;

        diff = transform.localEulerAngles - lastEulerAngles;
        angularVelocity = diff / Time.deltaTime;
        lastEulerAngles = transform.eulerAngles;
    }

    private void AddJoint() {
        joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = 20000;
        joint.breakTorque = 20000;
    }

    #region Interaction
    public void InteractWithObject() {
        if (IsGrabbed) {
            Release();
            return;
        }

        Interactable = coll.GetGrab();
        if (Interactable == null) {
            return;
        }

        if (Interactable.Type == InteractableType.Grabbable) {
            Grab();
        } else if (Interactable.Type == InteractableType.Interactable) {
            Interactable.Interact(this);
        } else if (Interactable.Type == InteractableType.GrabbableAndInteractable) {
            Grab();
        }
    }

    public void UninteractWithObject() {
        if (IsGrabbed) {
            if (VRControlSettings.HoldToGrab) {
                Release();
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

        if (Interactable.Type == InteractableType.GrabbableAndInteractable) {
            Interactable.Interact(this);
        }
    }

    public void GrabUninteract() {
        if (!IsGrabbed) {
            return;
        }

        Interactable = coll.GetGrab();

        if (Interactable.Type == InteractableType.GrabbableAndInteractable) {
            Interactable.Uninteract(this);
        }
    }
    #endregion

    private void Grab() {
        GrabObject();

        if (GrabbedRigidbody == null) {
            return;
        }

        // Fix later
        // GrabbedRigidbody.transform.position = transform.GetChild(0).position;

        GrabbedRigidbody.GetComponent<Interactable>().State = InteractState.Grabbed;

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedRigidbody.gameObject));

        if (other.IsGrabbed && other.GrabbedRigidbody.gameObject == GrabbedRigidbody.gameObject) {
            other.Release();
        }

        IsGrabbed = true;
        InitializeOffset();
        InitVelocities();
        AttachGrabbedObject();
    }
    
    private void OnJointBreak(float breakForce) {
        Logger.Print("Joint force broken: " + breakForce);
        Release();
    }

    public void Release() {
        IsGrabbed = false;

        DeattachGrabbedObject();

        if (GrabbedRigidbody == null) {
            return;
        }

        GrabbedRigidbody.GetComponent<Interactable>().State = InteractState.Grabbed;

        ItemPlacement.ReleaseSafely(GrabbedRigidbody.gameObject);

        GrabbedRigidbody.velocity = controls.Skeleton.velocity;
        GrabbedRigidbody.angularVelocity = controls.Skeleton.angularVelocity;
    }

    #region Helper functions
    private void AttachGrabbedObject() {
        Joint.connectedBody = GrabbedRigidbody;
    }

    private void DeattachGrabbedObject() {
        Joint.connectedBody = null;
    }

    private void InitializeOffset() {
        grabOffset = GrabbedRigidbody.transform.position - ColliderPosition;
        rotOffset = GrabbedRigidbody.transform.eulerAngles - ColliderEulerAngles;

        Logger.Print("Grab offset: " + grabOffset);
    }

    private void InitVelocities() {
        lastPos = transform.position;
        lastEulerAngles = transform.eulerAngles;
    }

    private Vector3 ColliderPosition {
        get {
            return transform.GetChild(0).transform.position;
        }
    }
    private Vector3 ColliderEulerAngles {
        get {
            return transform.eulerAngles;
        }
    }

    private void GrabObject() {
        GrabbedRigidbody = Interactable.GetComponent<Rigidbody>();
    }
    #endregion
}
