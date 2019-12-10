using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSmoother : MonoBehaviour {

    public Hand Hand { get; set; }
    private Transform target;
    private Rigidbody rb;

    [SerializeField]
    private float force = 10;

    [SerializeField]
    private float distanceLimit = 0.02f;
    [SerializeField]
    private float slowDownFactor = 0.7f;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        target = Hand.GetOffset();
    }

    private bool IsClose {
        get {
            return Vector3.Distance(transform.position, target.position) < distanceLimit;
        }
    }

    private void FixedUpdate() {
        if (IsClose) {
            KinematicAccurateMove();
            SlowDown();
        }
    }

    private void Update() {

        if (!IsClose) {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
        }

        LerpRotation();
    }

    private void LateUpdate() {
        if (!IsClose) {
            Stop();
        }
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
    private void KinematicAccurateMove() {
        float factor = Vector3.Distance(transform.position, target.position) / distanceLimit;
        transform.position = Vector3.Lerp(transform.position, target.position, factor);
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
