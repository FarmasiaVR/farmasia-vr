//using UnityEngine;

//public class RigidbodyGrab : GrabFunctionality {

//    #region fields
//    public bool IsGrabbed { get; set; }

//    private FixedJoint joint;
//    private FixedJoint Joint {
//        get {
//            if (joint == null) {
//                AddJoint();
//            }
//            return joint;
//        }
//    }

//    public HandCollider Coll { get; private set; }


//    public Rigidbody GrabbedRigidbody { get; private set; }
//    private Vector3 grabOffset;
//    private Vector3 rotOffset;

//    public Hand Hand { get; set; }
//    #endregion

//    private void AddJoint() {
//        joint = gameObject.AddComponent<FixedJoint>();
//        joint.breakForce = 20000;
//        joint.breakTorque = 20000;
//    }

//    #region Interaction overrides
//    public override void Interact(Interactable interactable) {
//        Grab();
//    }
//    public override void Uninteract(Interactable interactable) {
//        Release();
//    }

//    public override void GrabInteract(Interactable interactable) {
//        if (interactable.Types.AreOn(InteractableType.Grabbable, InteractableType.Interactable)) {
//            interactable.Interact(Hand);
//        }
//    }

//    public override void GrabUninteract(Interactable interactable) {
//        if (interactable.Types.AreOn(InteractableType.Grabbable, InteractableType.Interactable)) {
//            interactable.Uninteract(Hand);
//        }
//    }
//    #endregion

//    private void Grab() {
//        GrabObject();

//        if (GrabbedRigidbody == null) {
//            return;
//        }

//        // Fix later
//        // GrabbedRigidbody.transform.position = transform.GetChild(0).position;

//        GrabbedRigidbody.GetComponent<Interactable>().State = InteractState.Grabbed;

//        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedRigidbody.gameObject));

//        if (other.IsGrabbed && other.GrabbedRigidbody.gameObject == GrabbedRigidbody.gameObject) {
//            other.Release();
//        }

//        IsGrabbed = true;
//        InitializeOffset();
//        InitVelocities();
//        AttachGrabbedObject();
//    }

//    private void OnJointBreak(float breakForce) {
//        Logger.Print("Joint force broken: " + breakForce);
//        Release();
//    }

//    public void Release() {
//        IsGrabbed = false;

//        DeattachGrabbedObject();

//        if (GrabbedRigidbody == null) {
//            return;
//        }

//        GrabbedRigidbody.GetComponent<Interactable>().State = InteractState.None;

//        ItemPlacement.ReleaseSafely(GrabbedRigidbody.gameObject);

//        GrabbedRigidbody.velocity = controls.Skeleton.velocity;
//        GrabbedRigidbody.angularVelocity = controls.Skeleton.angularVelocity;
//    }

//    private void AttachGrabbedObject() {
//        Joint.connectedBody = GrabbedRigidbody;
//    }

//    private void DeattachGrabbedObject() {
//        Joint.connectedBody = null;
//    }

//    private void InitializeOffset() {
//        grabOffset = GrabbedRigidbody.transform.position - ColliderPosition;
//        rotOffset = GrabbedRigidbody.transform.eulerAngles - ColliderEulerAngles;

//        Logger.Print("Grab offset: " + grabOffset);
//    }

//    private void InitVelocities() {
//        lastPos = transform.position;
//        lastEulerAngles = transform.eulerAngles;
//    }

//    private Vector3 ColliderPosition {
//        get {
//            return transform.GetChild(0).transform.position;
//        }
//    }
//    private Vector3 ColliderEulerAngles {
//        get {
//            return transform.eulerAngles;
//        }
//    }

//    private void GrabObject() {
//        GrabbedRigidbody = Interactable.GetComponent<Rigidbody>();
//    }

//    public static Hand GrabbingHand(Rigidbody rb) {
//        foreach (VRHandControls controls in VRInput.Hands) {
//            if (rb == controls.Hand.GrabbedRigidbody) {
//                return controls.Hand;
//            }
//        }

//        return null;
//    }
//}