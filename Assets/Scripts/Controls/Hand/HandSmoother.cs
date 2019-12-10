using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSmoother : MonoBehaviour {

    public Hand Hand { get; set; }
    private Transform target;
    private Rigidbody rb;

    private float force;
    private float distanceLimit = 0.1f;
    private float slowDownFactor = 0.85f;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        target = Hand.Offset;
    }

    private bool IsClose {
        get {
            return Vector3.Distance(transform.position, target.position) < distanceLimit;
        }
    }

    private void FixedUpdate() {
        if (IsClose) {
            AccurateMove();
            SlowDown();
        } else {
            Stop();
        }
    }

    private void Update() {
        LerpRotation();
    }
    private void LerpRotation() {
        // FRAMERATE DEPENDENT
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 0.5f);
    }

    private void AccurateMove() {

        float factor = Vector3.Distance(transform.position, target.position) / distanceLimit;

        Vector3 direction = (target.position - transform.position).normalized;

        rb.AddForce(direction * factor * force);
    }
    private void SlowDown() {
        rb.velocity = rb.velocity * slowDownFactor;
        rb.angularVelocity = Vector3.zero;
    }

    private void Stop() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
