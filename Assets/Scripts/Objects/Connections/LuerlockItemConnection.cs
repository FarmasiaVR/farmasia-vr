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
        // Logger.Print(string.Format("Throwing LuerlockItemConnection, interactable: {0}", interactable));
        if (interactable != null && interactable.State == InteractState.Grabbed) {
            // Logger.Print("Throwing luerlock");
            var handType = Hand.GrabbingHand(interactable).HandType;

            connectedRB.velocity = VRInput.Skeleton(handType).velocity;
            connectedRB.angularVelocity = VRInput.Skeleton(handType).angularVelocity;
        }
        Destroy(joint);
    }

    public static LuerlockItemConnection Configuration(ItemConnector connector, Transform hand, Interactable interactable) {
        if (interactable.State == InteractState.LuerlockAttached) {
            return ConnectableItemConfiguration(connector, hand, interactable, interactable.Interactors.LuerlockPair.Value);
        } else if (interactable.State == InteractState.NeedleAttached) {
            return ConnectableItemConfiguration(connector, hand, interactable, interactable.Interactors.Needle);
        } else if (interactable.State == InteractState.LidAttached) {
            return ConnectableItemConfiguration(connector, hand, interactable, interactable.Interactors.AgarPlateLid);
        } else if (interactable.State == InteractState.PumpFilterAttached) {
            return ConnectableItemConfiguration(connector, hand, interactable, interactable.Interactors.PumpFilter);
        } else if (interactable.State == InteractState.CapAttached) {
            return ConnectableItemConfiguration(connector, hand, interactable, interactable.Interactors.BottleCap);
        }

        throw new Exception("No such configuration type for InteractState");
    }

    private static LuerlockItemConnection ConnectableItemConfiguration(ItemConnector connector, Transform hand, Interactable interactable, Interactable interactor) {

        Rigidbody targetRB = hand.GetComponent<Rigidbody>();
        Rigidbody capRB = interactor.Rigidbody;

        if (targetRB == null || capRB == null) {
            throw new System.Exception("Both parties did not have rigidbody");
        }

        LuerlockItemConnection conn = interactable.gameObject.AddComponent<LuerlockItemConnection>();

        conn.Connector = connector;
        conn.connectedRB = capRB;
        conn.interactable = interactable;

        Joint joint = JointConfiguration.AddJoint(targetRB.gameObject, capRB.mass);
        joint.connectedBody = capRB;

        conn.joint = joint;

        JointBreakSubscription.Subscribe(hand.gameObject, conn.JointBreak);

        return conn;
    }
}