using System;
using UnityEngine;

public class LuerlockItemConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Joint joint;
    private Interactable interactable;
    private Rigidbody connectedRB;
    #endregion

    private void JointBreak(float force) {
        Remove();
    }

    protected override void OnRemoveConnection() {
        Logger.Print(string.Format("Throwing LuerlockItemConnection, interactable: {0}", interactable));
        if (interactable != null && interactable.State == InteractState.Grabbed) {
            Logger.Print("Throwing luerlock");
            var handType = Hand.GrabbingHand(interactable).HandType;

            connectedRB.velocity = VRInput.Skeleton(handType).velocity;
            connectedRB.angularVelocity = VRInput.Skeleton(handType).angularVelocity;
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
        conn.connectedRB = luerlockRB;
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
        conn.connectedRB = needleRB;
        conn.interactable = interactable;

        Joint joint = JointConfiguration.AddJoint(targetRB.gameObject, needleRB.mass);
        joint.connectedBody = needleRB;

        conn.joint = joint;

        JointBreakSubscription.Subscribe(hand.gameObject, conn.JointBreak);

        return conn;
    }
}