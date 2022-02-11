using System;
using UnityEngine;

public static class ConnectionHandler {

    #region Attaching
    public static void GrabItem(ItemConnector connector, Transform target, Interactable addTo) {
        connector.Connection = ItemConnection.AddJointConnection(connector, target, addTo);
    }


    public static void GrabLuerlockAttachedItem(ItemConnector connector, Transform target, Interactable addTo) {
        connector.Connection = ItemConnection.AddLuerlockItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {
        connector.Connection = ItemConnection.AddLuerlockLooseItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {
        connector.Connection = ItemConnection.AddLuerlockLooseItemConnection(connector, target, addTo);
    }

    // Verify for Luerlock/Needle
    public static void GrabLuerlockWhenAttachedItemsAreGrabbed(ItemConnector connector, Transform target, Interactable addTo) {

        LuerlockAdapter luerlock = addTo as LuerlockAdapter;

        if (luerlock == null) {
            throw new System.Exception("Luerlock is null");
        }

        Interactable otherItem;
        if (luerlock.LeftConnector.HasAttachedObject && luerlock.LeftConnector.AttachedInteractable.State == InteractState.Grabbed) {
            otherItem = luerlock.LeftConnector.AttachedInteractable;
        } else if (luerlock.RightConnector.HasAttachedObject && luerlock.RightConnector.AttachedInteractable.State == InteractState.Grabbed) {
            otherItem = luerlock.RightConnector.AttachedInteractable;
        } else {
            Logger.Error("Could not find the other grabbed item");
            return;
        }

        Hand otherHand = Hand.GrabbingHand(otherItem);

        otherHand.Connector.Connection.Remove();

        HandSmoother smooth = target.GetComponent<Hand>().Smooth;
        Transform handOffset = smooth.transform;
        target = handOffset ?? target;

        connector.Connection = ItemConnection.AddJointConnection(connector, target, addTo);
        smooth?.DisableInitMode();

        otherHand.InteractWith(otherItem, false);
    }

    public static void GrabNeedleWhenAttachedItemIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {

        Needle needle = addTo as Needle;

        if (needle == null) {
            throw new System.Exception("Needle is null");
        }

        Interactable otherItem = needle.Connector.AttachedInteractable;

        Hand otherHand = Hand.GrabbingHand(otherItem);

        otherHand.Connector.Connection.Remove();

        HandSmoother smooth = target.GetComponent<Hand>().Smooth;
        Transform handOffset = smooth?.transform;
        target = handOffset ?? target;

        connector.Connection = ItemConnection.AddJointConnection(connector, target, addTo);
        smooth?.DisableInitMode();

        otherHand.InteractWith(otherItem, false);
    }
    public static void GrabLidWhenAttachedItemIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {

        AgarPlateLid lid = addTo as AgarPlateLid;

        if (lid == null) {
            throw new System.Exception("Needle is null");
        }

        Interactable otherItem = lid.Connector.AttachedInteractable;

        Hand otherHand = Hand.GrabbingHand(otherItem);

        otherHand.Connector.Connection.Remove();

        HandSmoother smooth = target.GetComponent<Hand>().Smooth;
        Transform handOffset = smooth?.transform;
        target = handOffset ?? target;

        connector.Connection = ItemConnection.AddJointConnection(connector, target, addTo);
        smooth?.DisableInitMode();

        otherHand.InteractWith(otherItem, false);
    }
    #endregion

    #region Releasing
    // Verify for Luerlock/Needle
    public static void ReleaseLuerlockWhenLuerlockAttachedItemIsGrabbed(LuerlockAdapter luerlock) {

        Interactable otherInteractable = null;

        foreach (Interactable other in luerlock.AttachedInteractables) {
            if (other.State == InteractState.Grabbed) {
                otherInteractable = other;
            }
        }

        Hand otherHand = Hand.GrabbingHand(otherInteractable);
        otherHand.Connector.Connection.Remove();
        otherHand.InteractWith(otherInteractable, false);
    }
    public static void ReleaseNeedleWhenNeedleAttachedItemIsGrabbed(Needle needle) {

        Interactable otherInteractable = needle.Connector.AttachedInteractable;

        Hand otherHand = Hand.GrabbingHand(otherInteractable);
        otherHand.Connector.Connection.Remove();
        otherHand.InteractWith(otherInteractable, false);
    }

    public static void ReleaseLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(Interactable grabbedInteractable, LuerlockAdapter luerlock) {

        Interactable otherInteractable = luerlock.GetOtherInteractable(grabbedInteractable);

        Hand otherHand = Hand.GrabbingHand(otherInteractable);
        otherHand.Connector.Connection.Remove();
        otherHand.InteractWith(otherInteractable);
    }
    #endregion
}