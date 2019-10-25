using UnityEngine;

public class HandConnector : ItemConnector {

    #region Fields
    public Hand Hand { get; private set; }

    public bool IsGrabbed { get => GrabbedInteractable != null; }

    public Interactable GrabbedInteractable { get; private set; }
    private Rigidbody grabbedRigidbody;

    private Vector3 grabPosOffset;
    private Vector3 grabRotOffset;

    private ItemConnection connection;
    #endregion

    public HandConnector(Hand hand) : base(hand.transform) {
        Hand = hand;
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable, int options) {
        if (interactable.GetComponent<Rigidbody>() == null) {
            Logger.Error("Interactable has no rigidbody");
            return;
        }

        // release item from other hand
        bool isGrabbingSameObject = interactable == Hand.Other.Connector.GrabbedInteractable;
        if (isGrabbingSameObject) {
            Hand.GrabbingHand(interactable.Rigidbody).Connector.ReleaseItem(0);
        }

        GrabbedInteractable = interactable;
        GrabbedInteractable.State.On(InteractState.Grabbed);
        GrabbedInteractable.Interactors.SetHand(Hand);
        grabbedRigidbody = interactable.GetComponent<Rigidbody>();

        InitializeOffset(grabbedRigidbody.transform);
        AttachGrabbedObject(interactable);

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(grabbedRigidbody.gameObject));
    }

    private void InitializeOffset(Transform current) {
        grabPosOffset = current.position - Hand.ColliderPosition;
        grabRotOffset = current.eulerAngles - Hand.transform.eulerAngles;
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
            connection = SmoothConnection.AddSmoothConnection(this, Hand.Offset, grabbedRigidbody.gameObject);
        } else {
            connection = ItemConnection.AddRigidConnection(this, Hand.Offset, grabbedRigidbody.gameObject);
        }
    }
    #endregion

    #region Releasing
    public override void ReleaseItem(int options) {
        if (!IsGrabbed) {
            Logger.Error("ReleaseItem(): Invalid state (is not grabbíng)");
            return;
        }

        if (GrabbedInteractable.State != InteractState.Grabbed) {
            Logger.Error("ReleaseItem(): Invalid state (item is not grabbed)");
            return;
        }

        DeattachGrabbedObject();
        ItemPlacement.ReleaseSafely(grabbedRigidbody.gameObject);

        grabbedRigidbody.velocity = VRInput.Skeleton(Hand.HandType).velocity;
        grabbedRigidbody.angularVelocity = VRInput.Skeleton(Hand.HandType).angularVelocity;
        grabbedRigidbody = null;

        GrabbedInteractable.State.Off(InteractState.Grabbed);
        GrabbedInteractable = null;
    }

    private void DeattachGrabbedObject() {
        MonoBehaviour.Destroy(connection);
    }
    #endregion
}
