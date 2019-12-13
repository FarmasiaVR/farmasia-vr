using System;
using UnityEngine;

public class LuerlockItemConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Transform target;
    private Joint joint;
    private Interactable interactable;
    #endregion

    private void JointBreak(float force) {
        Remove();
    }

    protected override void OnRemoveConnection() {
        if (interactable != null && interactable.State == InteractState.Grabbed) {
            var handType = Hand.GrabbingHand(interactable).HandType;
            Rigidbody luerlockRB = interactable.Interactors.LuerlockPair.Value.Rigidbody;

            luerlockRB.velocity = VRInput.Skeleton(handType).velocity;
            luerlockRB.angularVelocity = VRInput.Skeleton(handType).angularVelocity;
        }
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

        Rigidbody handRB = hand.GetComponent<Rigidbody>();
        Rigidbody luerlockRB = interactable.Interactors.LuerlockPair.Value.Rigidbody;

        if (handRB == null || luerlockRB == null) {
            throw new System.Exception("Both parties did not have rigidbody");
        }

        LuerlockItemConnection conn = interactable.gameObject.AddComponent<LuerlockItemConnection>();

        conn.Connector = connector;
        conn.target = hand;
        conn.interactable = interactable;

        Joint joint = JointConfiguration.AddJoint(handRB.gameObject, luerlockRB.mass);
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

        Joint joint = JointConfiguration.AddJoint(targetRB.gameObject, needleRB.mass);
        joint.connectedBody = needleRB;

        conn.joint = joint;

        JointBreakSubscription.Subscribe(hand.gameObject, conn.JointBreak);

        return conn;
    }
}