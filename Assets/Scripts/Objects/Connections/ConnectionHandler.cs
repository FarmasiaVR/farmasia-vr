using System;
using UnityEngine;

public static class ConnectionHandler {

    #region Attaching
    public static void GrabItem(ItemConnector connector, Transform target, Interactable addTo) {
        Logger.Print("Grab item");
        connector.Connection = ItemConnection.AddSmoothConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItem(ItemConnector connector, Transform target, Interactable addTo) {
        Logger.Print("Grab luerlock item");
        connector.Connection = ItemConnection.AddLuerlockItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {
        connector.Connection = ItemConnection.AddLuerlockLooseItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {
        connector.Connection = ItemConnection.AddLuerlockLooseItemConnection(connector, target, addTo);
    }

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

        connector.Connection = ItemConnection.AddSmoothConnection(connector, target, addTo);

        otherHand.InteractWith(otherItem);
    }
    #endregion

    #region Releasing
    public static void ReleaseLuerlockWhenLuerlockAttachedItemIsGrabbed(LuerlockAdapter luerlock) {

        Logger.Print("Rlease luerlock when luerlock attache item is grabbed");

        Interactable otherInteractable = null;

        foreach (Interactable other in luerlock.AttachedInteractables) {
            if (other.State == InteractState.Grabbed) {
                otherInteractable = other;
            }
        }

        Hand otherHand = Hand.GrabbingHand(otherInteractable);
        otherHand.Connector.Connection.Remove();
        otherHand.InteractWith(otherInteractable);
    }

    public static void ReleaseLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(Interactable grabbedInteractable, LuerlockAdapter luerlock) {

        Logger.Print("ReleaseLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed");

        Interactable otherInteractable = luerlock.GetOtherInteractable(grabbedInteractable);

        Hand otherHand = Hand.GrabbingHand(otherInteractable);
        otherHand.Connector.Connection.Remove();
        otherHand.InteractWith(otherInteractable);
    }
    #endregion
}