using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothConnection : MonoBehaviour {

    private ItemConnector connector;

    private Transform target;
    private Vector3 posOffset, rotOffset;

    private Rigidbody rb;

    private float maxDistance = 0.3f;

    private float maxForceFactor = 100000;
    private float maxForce;

    private float maxRotateForceFactor = 10000;
    private float maxRotateForce;

    private float brakeFactor = 0.75f;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        if (rb == null) {
            throw new System.Exception("Rigidbody was null");
        }

        rb.maxAngularVelocity = Mathf.Infinity;

        rb.useGravity = false;
        maxForce = maxForceFactor * rb.mass;
        maxRotateForce = maxRotateForceFactor * rb.mass;
    }

    private void Update() {
        CheckBreakCondition();
        Move();
        Rotate();
        Brake();
    }

    private void CheckBreakCondition() {
        float distance = Vector3.Distance(rb.transform.position, TargetPos);
        if (distance > maxDistance) {
            BreakConnection();
        }
    }

    private void BreakConnection() {
        connector.ReleaseItem(0);
    }

    private void Move() {

        float factor = Vector3.Distance(transform.position, TargetPos) / maxDistance;
        Vector3 direction = TargetPos - transform.position;

        Logger.PrintVariables("direction", direction, "factor", factor, "maxForce", maxForce);

        Vector3 force = direction.normalized * factor * maxForce * Time.deltaTime;

        Logger.PrintVariables("Force", force);

        rb.AddForce(force);
    }

    private void Rotate() {

        Quaternion required = Quaternion.FromToRotation(transform.eulerAngles, TargetRot);

        rb.AddTorque(required.eulerAngles * maxRotateForce * Time.deltaTime);
    }

    private void Brake() {
        rb.velocity = rb.velocity * brakeFactor;
    }

    private Vector3 TargetPos {
        get {
            return target.position + posOffset;
        }
    }
    private Vector3 TargetRot {
        get {
            return transform.eulerAngles + rotOffset;
        }
    }

    public static SmoothConnection AttachItem(ItemConnector connector, Transform target, GameObject addTo, Vector3 posOffset, Vector3 rotOffset) {

        SmoothConnection conn = addTo.gameObject.AddComponent<SmoothConnection>();

        conn.connector = connector;
        conn.target = target;
        conn.posOffset = posOffset;
        conn.rotOffset = rotOffset;

        return conn;
    }
}
