using UnityEngine;

public class HandConnector : ItemConnector {

    #region Fields
    public Hand Hand { get; private set; }

    public bool IsGrabbed { get => GrabbedInteractable != null; }

    public Interactable GrabbedInteractable { get; private set; }

    private Vector3 grabPosOffset;
    private Vector3 grabRotOffset;

    public override ItemConnection Connection { get; set; }
    #endregion

    public HandConnector(Hand hand) : base(hand.transform) {
        Hand = hand;
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable) {
        if (interactable.Rigidbody == null) {
            Logger.Warning("Interactable has no rigidbody");
        }

        // release item from other hand
        bool isGrabbingSameObject = interactable == Hand.Other.Connector.GrabbedInteractable;
        if (isGrabbingSameObject) {
            Hand.GrabbingHand(interactable).Connector.Connection.Remove();
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

    #if UNITY_NONVRCOMPUTER
        Connection = ItemConnection.AddRigidConnection(this, Hand.Offset, interactable.gameObject);
    #else

        if (interactable.State == InteractState.LuerlockAttached) {

            LuerlockAdapter luerlock = interactable.Interactors.LuerlockPair.Value;

            if (luerlock.ObjectCount > 1) {

                Interactable other = luerlock.GetOtherInteractable(interactable);

                if (other.State == InteractState.Grabbed) {
                    ConnectionHandler.GrabLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(this, Hand.transform, interactable);
                } else {
                    ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
                }
                
            } else {
                if (luerlock.State == InteractState.Grabbed) {
                    ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
                } else {
                    ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
                }
            }
        } else {
            ConnectionHandler.GrabItem(this, Hand.Offset, interactable);
        }
    #endif
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

        if (GrabbedInteractable.Rigidbody) {
            GrabbedInteractable.Rigidbody.velocity = VRInput.Skeleton(Hand.HandType).velocity;
            GrabbedInteractable.Rigidbody.angularVelocity = VRInput.Skeleton(Hand.HandType).angularVelocity;
        }

        GrabbedInteractable.State.Off(InteractState.Grabbed);
        GrabbedInteractable = null;
    }
#endregion
}
