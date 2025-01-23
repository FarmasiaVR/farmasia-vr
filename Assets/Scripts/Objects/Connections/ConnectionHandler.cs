﻿using System;
using UnityEngine;

public static class ConnectionHandler {

    #region Attaching
    public static void GrabItem(ItemConnector connector, Transform target, Interactable addTo) {
        // Logger.Print("ConnectionHandler GrabItem, target = " + target + " addTo = " + addTo);
        connector.Connection = ItemConnection.AddJointConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItem(ItemConnector connector, Transform target, Interactable addTo) {
        // Logger.Print("ConnectionHandler GrabLuerlockAttachedItem, target = " + target + " addTo = " + addTo);
        connector.Connection = ItemConnection.AddLuerlockItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {
        // Logger.Print("ConnectionHandler GrabLuerlockAttachedItemWhenLuerlockIsGrabbed, target = " + target + " addTo = " + addTo);
        connector.Connection = ItemConnection.AddLuerlockLooseItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(ItemConnector connector, Transform target, Interactable addTo) {
        // Logger.Print("ConnectionHandler GrabLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed, target = " + target + " addTo = " + addTo);
        connector.Connection = ItemConnection.AddLuerlockLooseItemConnection(connector, target, addTo);
    }

    // Verify for Luerlock/Needle
    public static void GrabLuerlockWhenAttachedItemsAreGrabbed(ItemConnector connector, Transform target, Interactable addTo) {
        // Logger.Print("ConnectionHandler GrabLuerlockWhenAttachedItemsAreGrabbed, target = " + target + " addTo = " + addTo);

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

    public static void GrabItemWhenAttachedItemIsGrabbed(ItemConnector connector, Transform target, ConnectableItem item) {
        Interactable otherItem = item.Connector.AttachedInteractable;

        Hand otherHand = Hand.GrabbingHand(otherItem);

        otherHand.Connector.Connection.Remove();

        HandSmoother smooth = target.GetComponent<Hand>().Smooth;
        Transform handOffset = smooth?.transform;
        target = handOffset ?? target;

        connector.Connection = ItemConnection.AddJointConnection(connector, target, item);
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
    public static void ReleaseItemWhenAttachedItemIsGrabbed(Interactable otherInteractable) {
        Logger.Print("Releasing items");
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