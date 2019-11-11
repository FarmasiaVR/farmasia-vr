using System;
using UnityEngine;

public class LuerlockItemConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Transform target;
    private Joint joint;
    #endregion

    private void JointBreak(float force) {
        Logger.PrintVariables("Joint broken", force);
        Remove();
    }

    protected override void RemoveConnection() {
        Destroy(joint);
    }

    public static LuerlockItemConnection Configuration(ItemConnector connector, Transform hand, Interactable interactable) {

        Rigidbody targetRB = hand.GetComponent<Rigidbody>();
        Rigidbody luerlockRB = interactable.Rigidbody;

        if (targetRB == null || luerlockRB == null) {
            throw new System.Exception("Both parties did not have rigidbody");
        }

        LuerlockItemConnection conn = interactable.gameObject.AddComponent<LuerlockItemConnection>();

        conn.Connector = connector;
        conn.target = hand;

        Joint joint = JointConfiguration.AddSpringJoint(targetRB.gameObject);
        joint.connectedBody = luerlockRB;

        conn.joint = joint;

        JointBreakSubscription.Subscribe(hand.gameObject, conn.JointBreak);

        return conn;
    }
}
