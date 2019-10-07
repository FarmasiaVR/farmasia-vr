using UnityEngine;

public class ItemConnectora {

    #region fields
    private const string luerlockTag = "Luerlock Position";

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

        if (!hand.IsGrabbed) {
            Logger.Print("Hand is not grabbíng");
        }

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
    public void ConnectItemToLuerlock(LuerlockAdapter luerlock, Interactable interactable, int side) {

        this.luerlock = luerlock;

        if (luerlock.State == InteractState.Grabbed) {
            Hand.GrabbingHand(luerlock.Rigidbody).Connector.ReleaseItemFromHand();
        }

        ReplaceObject(side, interactable?.gameObject);
    }

    private void ReplaceObject(int side, GameObject newObject) {

        GameObject colliderT = luerlock.Colliders[side];

        LuerlockAdapter.AttachedObject obj = luerlock.Objects[side];

        Logger.Print("ReplaceObject");
        if (obj.GameObject != null) {

            if (obj.GameObject == newObject) {
                return;
            }

            IgnoreCollisions(luerlock.transform, obj.GameObject.transform, false);

            // attachedObject.GameObject.AddComponent<Rigidbody>();
            obj.Rigidbody.isKinematic = false;
            // attachedObject.Rigidbody.WakeUp();
            obj.GameObject.transform.parent = null;
            obj.GameObject.transform.localScale = obj.Scale;
        }

        if (newObject == null) {
            obj.Interactable = null;
            luerlock.Objects[side] = obj;
            return;
        }

        obj.Interactable = newObject.GetComponent<Interactable>();
        

        obj.Scale = newObject.transform.localScale;

        // FIX
        if (Hand.GrabbingHand(luerlock.Rigidbody) != null) {
            Hand.GrabbingHand(obj.Rigidbody)?.Connector.ReleaseItemFromHand();
        } else {

            // ERRORS WILL COME HERE

        }

        IgnoreCollisions(luerlock.transform, obj.GameObject.transform, true);

        Vector3 newScale = new Vector3(
            obj.Scale.x / luerlock.transform.lossyScale.x,
            obj.Scale.y / luerlock.transform.lossyScale.y,
            obj.Scale.z / luerlock.transform.lossyScale.z);

        // Destroy(attachedObject.Rigidbody);
        obj.Rigidbody.isKinematic = true;
        //attachedObject.Rigidbody.Sleep();

        obj.GameObject.transform.parent = luerlock.transform;
        obj.GameObject.transform.localScale = newScale;
        obj.GameObject.transform.up = colliderT.transform.up;
        SetLuerlockPosition(colliderT, obj.GameObject.transform);

        luerlock.Objects[side] = obj;
    }

    private static void IgnoreCollisions(Transform a, Transform b, bool ignore) {

        Collider coll = a.GetComponent<Collider>();

        if (coll != null) {
            IgnoreCollisionsCollider(coll, b, ignore);
        }

        foreach (Transform child in a) {
            IgnoreCollisions(child, b, ignore);
        }
    }
    private static void IgnoreCollisionsCollider(Collider a, Transform b, bool ignore) {

        Collider coll = b.GetComponent<Collider>();

        if (coll != null) {
            Physics.IgnoreCollision(a, coll, ignore);
            foreach (Transform child in b) {
                IgnoreCollisionsCollider(a, child, ignore);
            }
        }
    }

    private void SetLuerlockPosition(GameObject collObject, Transform t) {

        Transform target = LuerlockAdapter.LuerlockPosition(t);

        if (target == null) {
            throw new System.Exception("Luerlock position not found");
        }

        Vector3 offset = collObject.transform.position - target.position;
        t.position += offset;
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