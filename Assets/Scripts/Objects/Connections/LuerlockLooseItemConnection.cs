using System;
using UnityEngine;

public class LuerlockLooseItemConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Transform luerlock;
    private Transform hand;
    #endregion

    private void OnJointBreak(float force) {
        Remove();
    }

    public static LuerlockLooseItemConnection Configuration(ItemConnector connector, Transform hand, GameObject interactable) {

        Rigidbody targetRB = hand.GetComponent<Rigidbody>();
        Rigidbody intRB = interactable.GetComponent<Rigidbody>();

        if (targetRB == null || intRB == null) {
            throw new System.Exception("Both parties did not have rigidbody");
        }

        LuerlockLooseItemConnection conn = interactable.AddComponent<LuerlockLooseItemConnection>();

        conn.Connector = connector;
        conn.hand = hand;
        conn.luerlock = ((LuerlockConnector)(connector)).Luerlock.transform;

        return conn;
    }
}
