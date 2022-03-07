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
        if (interactable.IsAttached) {
            // Logger.Print("Attaching attached");
            AttachAttached(interactable);
        } else if (interactable as GeneralItem is var generalItem && generalItem != null) {
            // Logger.Print("Attaching general");
            AttachGeneralItem(generalItem);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, interactable);
        }
    }

    private void AttachAttached(Interactable interactable) {
        if (interactable.State == InteractState.LuerlockAttached) {
            AttachLuerlockAttached(interactable);
        } else if (interactable.State == InteractState.NeedleAttached) {
            Logger.Print("Attaching interactable with needle");
            AttachNeedleAttached(interactable);
        } else if (interactable.State == InteractState.LidAttached) {
            Logger.Print("Attaching interactable with lid");
            AttachAgarplateLidAttached(interactable);
        } else if (interactable.State == InteractState.PumpFilterAttached) {
            Logger.Print("Attaching interactable with filter");
            AttachPumpFilterAttached(interactable);
        } else if (interactable.State == InteractState.CapAttached) {
            Logger.Print("Attaching interactable with cap");
            AttachCapAttached(interactable);
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

    private void AttachNeedleAttached(Interactable interactable) {
        Needle needle = interactable.Interactors.Needle;

        if (needle.State == InteractState.Grabbed) {
            ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
        }
        else {
            ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
        }
    }

    private void AttachAgarplateLidAttached(Interactable interactable) {
        AgarPlateLid lid = interactable.Interactors.AgarPlateLid;

        if (lid.State == InteractState.Grabbed) {
            ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
        }
        else {
            ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
        }
    }

    private void AttachPumpFilterAttached(Interactable interactable) {
        PumpFilter filter = interactable.Interactors.PumpFilter;

        if (filter.State == InteractState.Grabbed)
        {
            ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
        }
        else
        {
            ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
        }
    }

    private void AttachCapAttached(Interactable interactable) {
        BottleCap cap = interactable.Interactors.BottleCap;

        if (cap.State == InteractState.Grabbed) {
            ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
        } else {
            ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
        }
    }

    private void AttachGeneralItem(GeneralItem generalItem) {
        if (generalItem is LuerlockAdapter luerlock) {
            AttachLuerlock(luerlock);
        } else if (generalItem is Needle needle) {
            AttachNeedle(needle);
        } else if (generalItem is AgarPlateLid lid) {
            AttachAgarplateLid(lid);
        } else if (generalItem is PumpFilter filter) {
            AttachPumpFilter(filter);
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

    private void AttachNeedle(Needle needle) {
        if (needle.Connector.HasAttachedObject && needle.Connector.AttachedInteractable.State == InteractState.Grabbed) {
            ConnectionHandler.GrabNeedleWhenAttachedItemIsGrabbed(this, Hand.transform, needle);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, needle);
        }
    }

    private void AttachAgarplateLid(AgarPlateLid lid) {
        if (lid.Connector.HasAttachedObject && lid.Connector.AttachedInteractable.State == InteractState.Grabbed) {
            ConnectionHandler.GrabLidWhenAttachedItemIsGrabbed(this, Hand.transform, lid);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, lid);
        }
    }
    private void AttachPumpFilter(PumpFilter filter) {
        if (filter.Connector.HasAttachedObject && filter.Connector.AttachedInteractable.State == InteractState.Grabbed) {
            ConnectionHandler.GrabPumpFilterWhenAttachedItemIsGrabbed(this, Hand.transform, filter);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, filter);
        }
    }
    private void AttachCap(BottleCap cap) {
        if (cap.Connector.HasAttachedObject && cap.Connector.AttachedInteractable.State == InteractState.Grabbed) {
            ConnectionHandler.GrabPumpFilterWhenAttachedItemIsGrabbed(this, Hand.transform, cap);
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, cap);
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
