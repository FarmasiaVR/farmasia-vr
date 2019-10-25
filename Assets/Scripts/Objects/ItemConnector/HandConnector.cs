using UnityEngine;

public class HandConnector : ItemConnector {

    #region Fields
    public Hand Hand { get; private set; }

    public bool IsGrabbed { get => GrabbedRigidbody != null; }

    public Rigidbody GrabbedRigidbody { get; private set; }

    private Vector3 grabPosOffset;
    private Vector3 grabRotOffset;

    private ItemConnection connection;
    #endregion

    public HandConnector(Hand hand) : base(hand.transform) {
        Hand = hand;
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable, int options) {
        GrabbedRigidbody = interactable.GetComponent<Rigidbody>();
        if (GrabbedRigidbody == null) {
            Logger.Error("Interactable has no rigidbody");
            return;
        }

        // release item from other hand
        bool isGrabbingSameObject = GrabbedRigidbody == Hand.Other.Connector.GrabbedRigidbody;
        if (isGrabbingSameObject) {
            Hand.GrabbingHand(interactable.Rigidbody).Connector.ReleaseItem(0);
        }

        interactable.State.On(InteractState.Grabbed);
        interactable.Interactors.SetHand(Hand);

        InitializeOffset();
        AttachGrabbedObject(interactable);

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedRigidbody.gameObject));
    }

    private void InitializeOffset() {
        grabPosOffset = GrabbedRigidbody.transform.position - Hand.ColliderPosition;
        grabRotOffset = GrabbedRigidbody.transform.eulerAngles - Hand.transform.eulerAngles;
    }

    private bool AllowSmoothAttach(Interactable interactable) {
        if (interactable.Type != InteractableType.SmallObject) {
            return false;
        }

        bool isAttachedToLuerlock = interactable.State == InteractState.LuerlockAttached;
        LuerlockAdapter luerlock = isAttachedToLuerlock
                                    ? interactable.Interactors.LuerlockPair.Value
                                    : interactable as LuerlockAdapter;
        return luerlock == null || luerlock.ObjectCount == 0;
    }

    private void AttachGrabbedObject(Interactable interactable) {
        if (AllowSmoothAttach(interactable)) {
            connection = SmoothConnection.AddSmoothConnection(this, Hand.Offset, GrabbedRigidbody.gameObject);
        } else {
            connection = ItemConnection.AddRigidConnection(this, Hand.Offset, GrabbedRigidbody.gameObject);
        }
    }
    #endregion

    #region Releasing
    public override void ReleaseItem(int options) {
        if (!IsGrabbed) {
            Logger.Error("ReleaseItem(): Invalid state (is not grabbíng)");
            return;
        }

        if (Hand.GrabbedInteractable.State != InteractState.Grabbed) {
            Logger.Error("ReleaseItem(): Invalid state (item is not grabbed)");
            return;
        }

        if (GrabbedRigidbody == null) {
            Logger.Error("ReleaseItem(): Invalid state (current hand is not grabbing item)");
            return;
        }

        Interactable interactable = Interactable.GetInteractable(GrabbedRigidbody.transform);
        interactable.State.Off(InteractState.Grabbed);

        DeattachGrabbedObject();

        ItemPlacement.ReleaseSafely(GrabbedRigidbody.gameObject);
        GrabbedRigidbody.velocity = VRInput.Skeleton(Hand.HandType).velocity;
        GrabbedRigidbody.angularVelocity = VRInput.Skeleton(Hand.HandType).angularVelocity;
        GrabbedRigidbody = null;
    }

    private void DeattachGrabbedObject() {
        MonoBehaviour.Destroy(connection);
    }
    #endregion
}
