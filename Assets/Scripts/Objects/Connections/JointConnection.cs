﻿using UnityEngine;
using System;

public class JointConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    public Rigidbody rb;
    private Rigidbody target;
    private Transform transformTarget;
    public Interactable interactable;
    private Joint joint;

    #endregion

    private void Awake() {
        interactable = GetComponent<Interactable>();
        rb = interactable.Rigidbody;
    }

    private void SetJoint() {
        // debug this shit
        try {
            var s = "SetJoint: rb = " + rb.mass;

        } catch (NullReferenceException) {
            Logger.Warning("------ EPIC DEBUG STARTING --------");
            Logger.Print("The rb.mass call caused NRE. Interactable was " + interactable);
            Logger.Print("The Target RigidBody was " + target);
            Logger.Print("The transformTarget was " + transformTarget);
            Logger.Print("The Connector " + Connector.Connection);
        }
        joint = JointConfiguration.AddJoint(gameObject, rb.mass);
        joint.connectedBody = target;
    }

    private void OnJointBreak(float force) {
        Remove();
    }


    protected void Update() {
        if (transformTarget == null) {
            return;
        }
        NullCheck.Check(transform, transformTarget);
        transform.rotation = Quaternion.Lerp(transform.rotation, transformTarget.rotation, 0.5f);
    }

    protected override void OnRemoveConnection() {
        SetVelocity();
        Destroy(joint);
    }

    private void SetVelocity() {
       if (Connector as HandConnector is var handConnector && handConnector != null) {
            rb.velocity = VRInput.Skeleton(handConnector.Hand.HandType).velocity;
            rb.angularVelocity = VRInput.Skeleton(handConnector.Hand.HandType).angularVelocity;
        }
    }

    public static JointConnection Configuration(ItemConnector connector, Transform target, Interactable addTo) {

        NullCheck.Check(connector, target, addTo);

        Rigidbody targetRB = GetTargetRigidbody(target);

        JointConnection conn = addTo.gameObject.AddComponent<JointConnection>();

        conn.Connector = connector;
        conn.target = targetRB;
        conn.transformTarget = target;

        conn.SetJoint();

        return conn;
    }

    private static Rigidbody GetTargetRigidbody(Transform target) {

        Rigidbody rb = Interactable.GetInteractable(target)?.Rigidbody;

        if (rb != null) {
            return rb;
        }

        if (target.parent != null && target.parent.GetComponent<Hand>() != null) {
            return target.parent.GetComponent<Rigidbody>();
        }

        if (target.GetComponent<HandSmoother>() != null) {
            return target.GetComponent<Rigidbody>();
        }

        throw new System.Exception(string.Format("No target rigidbody found: {0}", target.name));
    }
}
