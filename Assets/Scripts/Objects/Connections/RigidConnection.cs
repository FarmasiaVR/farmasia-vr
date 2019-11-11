using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidConnection : ItemConnection {

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

        Logger.Print("Setting rigidbodies");

        Interactable interactable = Interactable.GetInteractable(rb.transform);
        LuerlockAdapter luerlock;

        if (interactable.State == InteractState.LuerlockAttached) {

            luerlock = interactable.Interactors.LuerlockPair.Value;

        } else {
            luerlock = interactable as LuerlockAdapter;
        }

        rb.useGravity = false;
        rigidbodies.Add(rb);

        if (luerlock == null) {
            return rb.mass;
        }

        float mass = luerlock.Rigidbody.mass;

        foreach (Rigidbody body in luerlock.AttachedRigidbodies) {
            mass += body.mass;
            body.useGravity = false;
            rigidbodies.Add(body);
        }

        return mass;
    }

    protected override void RemoveConnection() {
        ReleaseRigidbodies();
    }

    public void ReleaseRigidbodies() {

        Logger.Print("Enable gravity");

        foreach (Rigidbody r in rigidbodies) {
            r.useGravity = true;
        }
    }

    protected override void Update() {

        Stop();
        CheckBreakCondition();
        Move();
        Rotate();
    }

    private void Stop() {
        rb.velocity = Vector3.zero;
    }

    private void CheckBreakCondition() {

        float distance = Vector3.Distance(rb.transform.position, target.position);
        if (distance > maxDistance) {
            Remove();
        }
    }

    private void Move() {
        rb.MovePosition(target.position);
        //rb.position = target.position;
    }

    private void Rotate() {
        //rb.MoveRotation(Quaternion.Lerp(transform.rotation, target.rotation, 0.2f));
        rb.MoveRotation(target.rotation);
    }

    public static RigidConnection Configuration(ItemConnector connector, Transform target, Interactable addTo) {
        RigidConnection conn = addTo.gameObject.AddComponent<RigidConnection>();

        conn.Connector = connector;
        conn.target = target;

        return conn;
    }
}
