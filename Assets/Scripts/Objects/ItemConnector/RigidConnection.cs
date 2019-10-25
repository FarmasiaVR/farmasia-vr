using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidConnection: ItemConnection {

    protected override ItemConnector Connector { get; set; }

    private Transform target;

    private Rigidbody rb;

    private float maxDistance = 0.3f;

    private float rotationLerpFactor = 0.5f;

    private List<Rigidbody> rigidbodies;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        if (rb == null) {
            throw new System.Exception("Rigidbody was null");
        }

        rb.maxAngularVelocity = Mathf.Infinity;

        rigidbodies = new List<Rigidbody>();

        float mass = SetupRigidbodies();
    }

    private float SetupRigidbodies() {

        Interactable interactable = Interactable.GetInteractable(rb.transform);
        LuerlockAdapter luerlock;

        if (interactable.State == InteractState.LuerlockAttatch) {

            luerlock = interactable.Interactors.LuerlockPair.Value;

        } else {
            luerlock = interactable as LuerlockAdapter;
        }

        if (luerlock == null) {
            return rb.mass;
        }

        rb.useGravity = false;
        rigidbodies.Add(rb);
        float mass = luerlock.Rigidbody.mass;

        foreach (var obj in luerlock.Objects) {
            if (obj.Rigidbody != null) {
                mass += obj.Rigidbody.mass;
                obj.Rigidbody.useGravity = false;
                rigidbodies.Add(obj.Rigidbody);
            }
        }

        return mass;
    }

    private void OnDestroy() {
        ReleaseRigidbodies();
    }

    public void ReleaseRigidbodies() {
        foreach (Rigidbody r in rigidbodies) {
            r.useGravity = true;
        }
    }

    protected override void FixedUpdate() {
        CheckBreakCondition();
        Move();
        Rotate();
    }

    private void CheckBreakCondition() {

        float distance = Vector3.Distance(rb.transform.position, target.position);
        if (distance > maxDistance) {
            BreakConnection();
        }
    }

    private void BreakConnection() {
        Connector.ReleaseItem(0);
    }

    private void Move() {
        rb.MovePosition(target.position);
    }

    private void Rotate() {
        rb.MoveRotation(Quaternion.Lerp(transform.rotation, target.rotation, 0.2f));
    }

    public static RigidConnection Configuration(ItemConnector connector, Transform target, GameObject addTo) {
        RigidConnection conn = addTo.gameObject.AddComponent<RigidConnection>();

        conn.Connector = connector;
        conn.target = target;

        return conn;
    }
}
