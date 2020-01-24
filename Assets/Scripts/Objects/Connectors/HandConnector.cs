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

            if (interactable.State == InteractState.LuerlockAttached) {
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
            } else if (interactable.State == InteractState.NeedleAttached) {

                Needle needle = interactable.Interactors.Needle;

                if (needle == null) {
                    throw new Exception("Item is needle attached but needle was null");
                }

                if (needle.State == InteractState.Grabbed) {
                    ConnectionHandler.GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(this, Hand.transform, interactable);
                } else {
                    ConnectionHandler.GrabLuerlockAttachedItem(this, Hand.transform, interactable);
                }
            } else {
                throw new Exception("Interactrable State is attached but not attached");
            }
        } else if (interactable as GeneralItem is var generalItem && generalItem != null) {

            if (generalItem.ObjectType == ObjectType.Luerlock) {
                LuerlockAdapter luerlock = generalItem as LuerlockAdapter;
                if (luerlock.GrabbedObjectCount > 0) {
                    ConnectionHandler.GrabLuerlockWhenAttachedItemsAreGrabbed(this, Hand.transform, luerlock);
                } else {
                    ConnectionHandler.GrabItem(this, Hand.Smooth.transform, luerlock);
                }
            } else if (generalItem.ObjectType == ObjectType.Needle) {
                Needle needle = generalItem as Needle;
                if (needle.Connector.HasAttachedObject && needle.Connector.AttachedInteractable.State == InteractState.Grabbed) {
                    ConnectionHandler.GrabNeedleWhenAttachedItemIsGrabbed(this, Hand.transform, needle);
                } else {
                    ConnectionHandler.GrabItem(this, Hand.Smooth.transform, needle);
                }
            } else {
                ConnectionHandler.GrabItem(this, Hand.Smooth.transform, interactable);
            }
        } else {
            ConnectionHandler.GrabItem(this, Hand.Smooth.transform, interactable);
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
                ConnectionHandler.ReleaseNeedleWhenNeedleAttachedItemIsGrabbed(needle);
            }
        }
    }
    #endregion
}
