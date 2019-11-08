using System;
using UnityEngine;

public class LuerlockItemConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Transform target;
    private Joint joint;
    #endregion

    private void OnJointBreak(float force) {
        Remove();
    }

    protected override void RemoveConnection() {
        Destroy(joint);
    }

    public static LuerlockItemConnection Configuration(ItemConnector connector, Transform hand, GameObject interactable) {

        Rigidbody targetRB = hand.GetComponent<Rigidbody>();
        Rigidbody intRB = interactable.GetComponent<Rigidbody>();

        if (targetRB == null || intRB == null) {
            throw new System.Exception("Both parties did not have rigidbody");
        }

        LuerlockItemConnection conn = interactable.AddComponent<LuerlockItemConnection>();

        conn.Connector = connector;
        conn.target = hand;

        Joint joint = JointConfiguration.AddSpringJoint(conn.gameObject);
        joint.connectedBody = targetRB;

        conn.joint = joint;

        return conn;
    }
}
