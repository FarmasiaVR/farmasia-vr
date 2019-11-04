using UnityEngine;

public struct RigidbodyState {

    private float angularDrag;
    private Vector3 angularVelocity;
    private Vector3 centerOfMass;
    private CollisionDetectionMode collisionDetectionMode;
    private RigidbodyConstraints constraints;
    private bool detectCollisions;
    private float drag;
    private bool freezeRotation;
    private Vector3 inertiaTensor;
    private Quaternion inertiaTensorRotation;
    private RigidbodyInterpolation interpolation;
    private bool isKinematic;
    private float mass;
    private float maxAngularVelocity;
    private float maxDepenetrationVelocity;
    private Vector3 position;
    private Quaternion rotation;
    private float sleepThreshold;
    private int solverIterations;
    private int solverVelocityIterations;
    private bool useGravity;
    private Vector3 velocity;
    private Vector3 worldCenterOfMass;

    public enum EnableSettings {
        OnlyEnable,
        ResetToOriginalState,
        FullReset
    }

    public RigidbodyState(Rigidbody rb) {

        if (rb == null) {
            throw new System.Exception("State rigidbody is null");
        }

        this.angularDrag = rb.angularDrag;
        this.angularVelocity = rb.angularVelocity;
        this.centerOfMass = rb.centerOfMass;
        this.collisionDetectionMode = rb.collisionDetectionMode;
        this.constraints = rb.constraints;
        this.detectCollisions = rb.detectCollisions;
        this.drag = rb.drag;
        this.freezeRotation = rb.freezeRotation;
        this.inertiaTensor = rb.inertiaTensor;
        this.inertiaTensorRotation = rb.inertiaTensorRotation;
        this.interpolation = rb.interpolation;
        this.isKinematic = rb.isKinematic;
        this.mass = rb.mass;
        this.maxAngularVelocity = rb.maxAngularVelocity;
        this.maxDepenetrationVelocity = rb.maxDepenetrationVelocity;
        this.position = rb.position;
        this.rotation = rb.rotation;
        this.sleepThreshold = rb.sleepThreshold;
        this.solverIterations = rb.solverIterations;
        this.solverVelocityIterations = rb.solverVelocityIterations;
        this.useGravity = rb.useGravity;
        this.velocity = rb.velocity;
        this.worldCenterOfMass = rb.worldCenterOfMass;
    }

    public void SetRigidbody(Rigidbody rb) {
        SetValues(rb, EnableSettings.OnlyEnable);
    }
    public void SetValues(Rigidbody rb, EnableSettings settings) {

        rb.angularDrag = angularDrag;
        rb.centerOfMass = centerOfMass;
        rb.collisionDetectionMode = collisionDetectionMode;
        rb.constraints = constraints;
        rb.detectCollisions = detectCollisions;
        rb.drag = drag;
        rb.freezeRotation = freezeRotation;
        rb.inertiaTensor = inertiaTensor;
        rb.inertiaTensorRotation = inertiaTensorRotation;
        rb.interpolation = interpolation;
        rb.isKinematic = isKinematic;
        rb.mass = mass;
        rb.maxAngularVelocity = maxAngularVelocity;
        rb.maxDepenetrationVelocity = maxDepenetrationVelocity;
        rb.sleepThreshold = sleepThreshold;
        rb.solverIterations = solverIterations;
        rb.solverVelocityIterations = solverVelocityIterations;
        rb.useGravity = useGravity;

        if (settings == EnableSettings.OnlyEnable) {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        } else {
            if (settings == EnableSettings.ResetToOriginalState || settings == EnableSettings.FullReset) {
                rb.angularVelocity = angularVelocity;
                rb.velocity = velocity;
            }
            if (settings == EnableSettings.FullReset) {
                rb.position = position;
                rb.rotation = rotation;
            }
        }
    }
}