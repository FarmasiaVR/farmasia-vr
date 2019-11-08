using UnityEngine;

public class HandConnector : ItemConnector {

    #region Fields
    public Hand Hand { get; private set; }

    public bool IsGrabbed { get => GrabbedInteractable != null; }

    public Interactable GrabbedInteractable { get; private set; }

    private Vector3 grabPosOffset;
    private Vector3 grabRotOffset;

    public override ItemConnection Connection { get; protected set; }
    #endregion

    public HandConnector(Hand hand) : base(hand.transform) {
        Hand = hand;
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable) {
        if (interactable.Rigidbody == null) {
            Logger.Error("Interactable has no rigidbody");
            return;
        }

        // release item from other hand
        bool isGrabbingSameObject = interactable == Hand.Other.Connector.GrabbedInteractable;
        if (isGrabbingSameObject) {
            Hand.GrabbingHand(interactable.Rigidbody).Connector.Connection.Remove();
        }

        GrabbedInteractable = interactable;
        GrabbedInteractable.State.On(InteractState.Grabbed);
        GrabbedInteractable.Interactors.SetHand(Hand);

        InitializeOffset(GrabbedInteractable.transform);



        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedInteractable.gameObject));
        AttachGrabbedItem(GrabbedInteractable);
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
        return luerlock == null || !luerlock.HasAttachedObjects;
    }

    private void AttachGrabbedItem(Interactable interactable) {

        if (interactable.State == InteractState.LuerlockAttached) {
            // testing with joint connection instead of luerlock connectio -> cant detach
           // connection = ItemConnection.AddLuerlockItemConnection(this, Hand.Offset, interactable.gameObject);
            Connection = ItemConnection.AddJointConnection(this, Hand.transform, interactable.gameObject);
        } else {
            if (AllowSmoothAttach(interactable)) {
                Connection = ItemConnection.AddSmoothConnection(this, Hand.Offset, interactable.gameObject);
            } else {
                // Replace with spring JointConnection for better control
                Connection = ItemConnection.AddRigidConnection(this, Hand.Offset, interactable.gameObject);
            }
        }
    }
    #endregion

    #region Releasing
    public override void OnReleaseItem() {
        if (!IsGrabbed) {
            Logger.Error("ReleaseItem(): Invalid state (is not grabbíng)");
            return;
        }

        if (GrabbedInteractable.State != InteractState.Grabbed) {
            Logger.Error("ReleaseItem(): Invalid state (item is not grabbed)");
            return;
        }

        GrabbedInteractable.Rigidbody.velocity = VRInput.Skeleton(Hand.HandType).velocity;
        GrabbedInteractable.Rigidbody.angularVelocity = VRInput.Skeleton(Hand.HandType).angularVelocity;

        GrabbedInteractable.State.Off(InteractState.Grabbed);
        GrabbedInteractable = null;
    }
    #endregion
}
