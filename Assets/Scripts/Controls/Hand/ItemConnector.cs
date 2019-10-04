using UnityEngine;

public class ItemConnector {

    #region fields
    private Hand hand;
    private LuerlockAdapter luerlock;

    public bool IsGrabbed { get; private set; }

    public Rigidbody GrabbedRigidbody { get; private set; }

    private Vector3 grabOffset;
    private Vector3 rotOffset;

    private FixedJoint joint;
    private FixedJoint Joint {
        get {
            if (joint == null) {
                AddJoint(hand.gameObject);
            }
            return joint;
        }
    }
    #endregion

    #region Hand grab
    public void ConnectItemToHand(Hand hand, Interactable interactable) {

        this.hand = hand;

        if (interactable.State == InteractState.Grabbed) {
            hand.Other.Connector.ReleaseItemFromHand();
        }

        GrabbedRigidbody = interactable.GetComponent<Rigidbody>();

        if (GrabbedRigidbody == null) {
            throw new System.Exception("Interactable had no rigidbody");
        }

        GrabbedRigidbody.GetComponent<Interactable>().State.On(InteractState.Grabbed);

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedRigidbody.gameObject));

        IsGrabbed = true;
        InitializeOffset();
        AttachGrabbedObject();
    }

    private void InitializeOffset() {
        grabOffset = GrabbedRigidbody.transform.position - ColliderPosition;
        rotOffset = GrabbedRigidbody.transform.eulerAngles - ColliderEulerAngles;

        Logger.Print("Grab offset: " + grabOffset);
    }

    private void AttachGrabbedObject() {
        Joint.connectedBody = GrabbedRigidbody;
    }
    #endregion

    #region Hand release
    public void ReleaseItemFromHand() {

        if (hand.Interactable.State != InteractState.Grabbed) {
            throw new System.Exception("Trying to release ungrabbed item");
        }

        IsGrabbed = false;

        DeattachGrabbedObject();

        if (GrabbedRigidbody == null) {
            return;
        }

        GrabbedRigidbody.GetComponent<Interactable>().State.Off(InteractState.Grabbed);

        ItemPlacement.ReleaseSafely(GrabbedRigidbody.gameObject);

        GrabbedRigidbody.velocity = VRInput.Skeleton(hand.HandType).velocity;
        GrabbedRigidbody.angularVelocity = VRInput.Skeleton(hand.HandType).angularVelocity;
    }

    private void DeattachGrabbedObject() {
        Joint.connectedBody = null;
    }
    #endregion

    #region Luerlock grab
    public void ConnectItemToLuerlock(LuerlockAdapter luerlock, Interactable interactable) {

        this.luerlock = luerlock;

        if (luerlock.State == InteractState.Grabbed) {
            Hand.GrabbingHand(luerlock.Rigidbody).Connector.ReleaseItemFromHand();
        }


    }

    public void ReleaseItemFromLuerlock(int side, Interactable Interactable) {

        //if (luerlock.Interactable.State != InteractState.Grabbed) {
        //    throw new System.Exception("Trying to release ungrabbed item");
        //}


    }
    #endregion

    #region Helper functions
    private void AddJoint(GameObject gObject) {
        joint = gObject.AddComponent<FixedJoint>();
        joint.breakForce = 20000;
        joint.breakTorque = 20000;
    }

    private Vector3 ColliderPosition {
        get {
            return hand.transform.GetChild(0).transform.position;
        }
    }
    private Vector3 ColliderEulerAngles {
        get {
            return hand.transform.eulerAngles;
        }
    }
    #endregion
}