using System;
using UnityEngine;

public class HandConnector : ItemConnector {

    #region Fields
    public Hand Hand { get; private set; }

    public bool IsGrabbed { get => GrabbedInteractable != null; }

    public Interactable GrabbedInteractable { get; private set; }

    public override ItemConnection Connection { get; set; }
    #endregion

    public HandConnector(Hand hand) : base(hand.transform) {
        Hand = hand;
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable) {

        bool isGrabbingSameObject = interactable == Hand.Other.Connector.GrabbedInteractable;
        if (isGrabbingSameObject) {
            Hand.GrabbingHand(interactable).Connector.Connection.Remove();
        }

        GrabbedInteractable = interactable;
        GrabbedInteractable.State.On(InteractState.Grabbed);
        GrabbedInteractable.Interactors.SetHand(Hand);

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedInteractable.gameObject));
        AttachGrabbedItem(GrabbedInteractable);
    }

    private void AttachGrabbedItem(Interactable interactable) {
        if (interactable.IsAttached) {
            AttachAttached(interactable);
        } else if (interactable as GeneralItem is var generalItem && generalItem != null) {
            AttachGeneralItem(generalItem);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, interactable);
        }
    }

    private void AttachAttached(Interactable interactable) {
        if (interactable.State == InteractState.LuerlockAttached) {
            AttachLuerlockAttached(interactable);
        } else if (interactable.IsAttached) {
            AttachConnectableItemAttached(interactable);
        } else {
            throw new Exception("Interactrable State is attached but not attached");
        }
    }

    private void AttachLuerlockAttached(Interactable interactable) {
        LuerlockAdapter luerlock = interactable.Interactors.LuerlockPair.Value;
        if (luerlock.State == InteractState.Grabbed) {
            ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
        } else {
            if (luerlock.GrabbedObjectCount == 2) {
                ConnectionHandler.GrabLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(this, Hand.transform, interactable);
            } else {
                ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
            }
        }
    }

    private void AttachConnectableItemAttached(Interactable interactable) {
        ConnectableItem connectable = interactable.Interactors.ConnectableItem;
        if (connectable != null && connectable.State == InteractState.Grabbed) {
            ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
        } else {
            ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
        }
    }

    private void AttachGeneralItem(GeneralItem generalItem) {
        if (generalItem is LuerlockAdapter luerlock) {
            AttachLuerlock(luerlock);
        } else if (generalItem is ConnectableItem item) {
            AttachConnectableItem(item);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, generalItem);
        }
    }

    private void AttachLuerlock(LuerlockAdapter luerlock) {
        if (luerlock.GrabbedObjectCount > 0) {
            ConnectionHandler.GrabLuerlockWhenAttachedItemsAreGrabbed(this, Hand.transform, luerlock);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, luerlock);
        }
    }

    private void AttachConnectableItem(ConnectableItem item) {
        if (item.Connector.HasAttachedObject && item.Connector.AttachedInteractable.State == InteractState.Grabbed) {
            ConnectionHandler.GrabItemWhenAttachedItemIsGrabbed(this, Hand.transform, item);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, item);
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

        if (GrabbedInteractable.Rigidbody) {
            GrabbedInteractable.Rigidbody.velocity = VRInput.Skeleton(Hand.HandType).velocity;
            GrabbedInteractable.Rigidbody.angularVelocity = VRInput.Skeleton(Hand.HandType).angularVelocity;
        }

        GrabbedInteractable.State.Off(InteractState.Grabbed);

        SafeRelease();

        GrabbedInteractable = null;
    }

    private void SafeRelease() {
        Logger.Print("Something is being released");
        if (GrabbedInteractable.State == InteractState.LuerlockAttached) {

            LuerlockAdapter l = GrabbedInteractable.Interactors.LuerlockPair.Value;

            if (l.GrabbedObjectCount > 0) {
                ConnectionHandler.ReleaseLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(GrabbedInteractable, l);
                return;
            }
        }

        if (GrabbedInteractable as LuerlockAdapter is var luerlock && luerlock != null) {
            if (luerlock.GrabbedObjectCount > 0) {
                ConnectionHandler.ReleaseLuerlockWhenLuerlockAttachedItemIsGrabbed(luerlock);
            }
        } else if (GrabbedInteractable as Needle is var needle && needle != null) {
            if (needle.Connector.HasAttachedObject && needle.Connector.AttachedInteractable.State == InteractState.Grabbed) {
                ConnectionHandler.ReleaseItemWhenAttachedItemIsGrabbed(needle.Connector.AttachedInteractable);
            }
        } else if (GrabbedInteractable as AgarPlateLid is var lid && lid != null) {
            if (lid.Connector.HasAttachedObject && lid.Connector.AttachedInteractable.State == InteractState.Grabbed) {
                Logger.Print("Releasing agar plates part 2");
                ConnectionHandler.ReleaseItemWhenAttachedItemIsGrabbed(lid.Connector.AttachedInteractable);
            }
        } else if (GrabbedInteractable as PumpFilter is var filter && filter != null) {
            if (filter.Connector.HasAttachedObject && filter.Connector.AttachedInteractable.State == InteractState.Grabbed) {
                ConnectionHandler.ReleaseItemWhenAttachedItemIsGrabbed(filter.Connector.AttachedInteractable);
            }
        } else if (GrabbedInteractable as BottleCap is var cap && cap != null) {
            if (cap.Connector.HasAttachedObject && cap.Connector.AttachedInteractable.State == InteractState.Grabbed) {
                ConnectionHandler.ReleaseItemWhenAttachedItemIsGrabbed(cap.Connector.AttachedInteractable);
            }
        }
    }

    #endregion
}
