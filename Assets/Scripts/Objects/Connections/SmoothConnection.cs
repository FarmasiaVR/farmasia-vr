using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothConnection : ItemConnection {

    protected override ItemConnector Connector { get; set; }

    private Transform target;

    private Rigidbody rb;

    private float maxDistance = 0.3f;

    private float maxForceFactor = 100;
    private float maxForce;

    private float maxRotateForceFactor = 100;
    private float maxRotateForce;

    private float rotationLerpFactor = 0.5f;

    private float brakeFactor = 0.75f;

    private List<Rigidbody> rigidbodies;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        if (rb == null) {
            throw new System.Exception("Rigidbody was null");
        }

        rb.maxAngularVelocity = Mathf.Infinity;

        rigidbodies = new List<Rigidbody>();

        float mass = SetupRigidbodies();

        maxForce = maxForceFactor * mass;
        maxRotateForce = maxRotateForceFactor * mass;
    }

    private float SetupRigidbodies() {

        Interactable interactable = Interactable.GetInteractable(rb.transform);
        LuerlockAdapter luerlock;

        if (interactable.State == InteractState.LuerlockAttached) {

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
        Brake();
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

        float factor = Vector3.Distance(transform.position, target.position) / maxDistance;
        Vector3 direction = target.position - transform.position;

        Vector3 force = direction.normalized * factor * maxForce;

        rb.AddForce(force);
    }

    private void Rotate() {

        rb.MoveRotation(Quaternion.Lerp(transform.rotation, target.rotation, 0.2f));

        return;

        rb.MoveRotation(LerpRotation(transform.eulerAngles, target.eulerAngles));

        return;

        Quaternion required = Quaternion.FromToRotation(transform.eulerAngles, target.eulerAngles);

        Vector3 cross = Vector3.Cross(transform.eulerAngles, target.eulerAngles);

        Logger.PrintVariables("required", required.eulerAngles, "current", transform.eulerAngles, "target", target.eulerAngles, "force", maxRotateForce);

        Vector3 torque = cross * maxRotateForce;

        Logger.PrintVariables("Torque", torque, "angular vel", rb.angularVelocity);


        rb.AddTorque(torque);
    }

    private Quaternion LerpRotation(Vector3 from, Vector3 to) {

        Vector3 rot = Vector3.zero;

        rot.x = LerpFloat(from.x, to.x);
        rot.y = LerpFloat(from.y, to.y);
        rot.z = LerpFloat(from.z, to.z);

        return Quaternion.Euler(rot);
    }
    private float LerpFloat(float start, float end) {

        float diff = end - start;

        return start + diff * rotationLerpFactor;
    }


    private void Brake() {
        rb.velocity = rb.velocity * brakeFactor;
        rb.angularVelocity = Vector3.zero;
    }

    public static SmoothConnection Configuration(ItemConnector connector, Transform target, GameObject addTo) {
        SmoothConnection conn = addTo.gameObject.AddComponent<SmoothConnection>();

        conn.Connector = connector;
        conn.target = target;

        return conn;
    }
}
