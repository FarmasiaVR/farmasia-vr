using UnityEngine;

public class ItemConnector {

    #region fields
    private Hand hand;

    public bool IsGrabbed { get; private set; }

    private Rigidbody GrabbedRigidbody;

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

    public ItemConnector(Hand hand) {
        this.hand = hand;
    }

    #region Hand grab
    public void ConnectItemToHand(Interactable interactable) {

        GrabbedRigidbody = interactable.GetComponent<Rigidbody>();

        if (GrabbedRigidbody == null) {
            throw new System.Exception("Interactable had no rigidbody");
        }

        GrabbedRigidbody.GetComponent<Interactable>().State = InteractState.Grabbed;

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedRigidbody.gameObject));

        if (hand.Other.IsGrabbed && hand.Other.Connector.GrabbedRigidbody.gameObject == GrabbedRigidbody.gameObject) {
            hand.Other.Connector.ReleaseItemFromHand();
        }

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
        IsGrabbed = false;

        DeattachGrabbedObject();

        if (GrabbedRigidbody == null) {
            return;
        }

        GrabbedRigidbody.GetComponent<Interactable>().State = InteractState.None;

        ItemPlacement.ReleaseSafely(GrabbedRigidbody.gameObject);

        GrabbedRigidbody.velocity = VRInput.Skeleton(hand.HandType).velocity;
        GrabbedRigidbody.angularVelocity = VRInput.Skeleton(hand.HandType).angularVelocity;
    }

    private void DeattachGrabbedObject() {
        Joint.connectedBody = null;
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