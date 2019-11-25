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

    protected override void OnRemoveConnection() {
        Destroy(joint);
    }

    public static LuerlockItemConnection Configuration(ItemConnector connector, Transform hand, Interactable interactable) {
        if (interactable.State == InteractState.LuerlockAttached) {
            return LuerlockConfiguration(connector, hand, interactable);
        } else if (interactable.State == InteractState.NeedleAttached) {
            return NeedleConfiguration(connector, hand, interactable);
        }

        throw new Exception("No such configuration type for InteractState");
    }
    private static LuerlockItemConnection LuerlockConfiguration(ItemConnector connector, Transform hand, Interactable interactable) {

        Rigidbody targetRB = hand.GetComponent<Rigidbody>();
        Rigidbody luerlockRB = interactable.Interactors.LuerlockPair.Value.Rigidbody;

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
    private static LuerlockItemConnection NeedleConfiguration(ItemConnector connector, Transform hand, Interactable interactable) {

        Rigidbody targetRB = hand.GetComponent<Rigidbody>();
        Rigidbody needleRB = interactable.Interactors.Needle.Rigidbody;

        if (targetRB == null || needleRB == null) {
            throw new System.Exception("Both parties did not have rigidbody");
        }

        LuerlockItemConnection conn = interactable.gameObject.AddComponent<LuerlockItemConnection>();

        conn.Connector = connector;
        conn.target = hand;

        Joint joint = JointConfiguration.AddSpringJoint(targetRB.gameObject);
        joint.connectedBody = needleRB;

        conn.joint = joint;

        JointBreakSubscription.Subscribe(hand.gameObject, conn.JointBreak);

        return conn;
    }
}