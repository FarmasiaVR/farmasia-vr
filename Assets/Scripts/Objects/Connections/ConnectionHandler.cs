using UnityEngine;

public static class ConnectionHandler {

    #region Attaching
    public static void GrabItem(ItemConnector connector, Transform target, GameObject addTo) {
        Logger.Print("Grab item");
        connector.Connection = ItemConnection.AddSmoothConnection(connector, target, addTo);
    }

    public static void AttachItemToLuerlock(ItemConnector connector, Transform target, GameObject addTo) {
        throw new System.NotImplementedException();
    }

    public static void GrabLuerlockAttachedItem(ItemConnector connector, Transform target, GameObject addTo) {
        Logger.Print("Grab luerlock item");
        connector.Connection = ItemConnection.AddLuerlockItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenLuerlockIsGrabbed(ItemConnector connector, Transform target, GameObject addTo) {
        connector.Connection = ItemConnection.AddLuerlockLooseItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockAttachedItemWhenOtherLuerlockAttachedItemIsGrabbed(ItemConnector connector, Transform target, GameObject addTo) {

        Interactable interactable = addTo.GetComponent<Interactable>();
        Interactable other = ((LuerlockConnector)connector).Luerlock.GetOtherInteractable(interactable);

        // Replace other interactables connection with LuerlockTwoWay connection
        Hand otherHand = Hand.GrabbingHand(other);
        LuerlockAdapter luerlock = other.Interactors.LuerlockPair.Value;

        otherHand.Connector.Connection.Remove();
        otherHand.Connector.Connection = ItemConnection.AddLuerlockLooseTwoWayItemConnection(otherHand.Connector, otherHand.transform, other.gameObject);

        // Add connection to just grabbed item
        connector.Connection = ItemConnection.AddLuerlockLooseTwoWayItemConnection(connector, target, addTo);
    }

    public static void GrabLuerlockWhenAttachedItemsAreGrabbed(ItemConnector connector, Transform target, GameObject addTo) {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Releasing


    #endregion
}