using UnityEngine;

public class SpringJointConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Rigidbody rb;
    private Rigidbody target;
    private Transform rotationTarget;
    private Interactable interactable;
    private Joint joint;

    private float speedMultiplier = 0.85f;
    private float speedMultiplierDistanceLimit = 0.3f;

    private Vector3 lastPos;
    private Vector3 lastAngles;
    #endregion

    private void Awake() {
        interactable = GetComponent<Interactable>();
        rb = interactable.Rigidbody;
        rb.useGravity = false;
    }

    private void SetJoint() {

        joint = JointConfiguration.AddSpringJoint(gameObject);
        joint.connectedBody = target;
    }

    private void OnJointBreak(float force) {
        OnRemoveConnection();
    }

    protected void FixedUpdate() {

        float distance = Vector3.Distance(joint.anchor, transform.position);
        distance = distance > speedMultiplierDistanceLimit ? speedMultiplierDistanceLimit : distance;

        float factor = distance / speedMultiplierDistanceLimit;

        float multiplier = 1 - (1 - speedMultiplier) * factor;

        rb.velocity = rb.velocity * multiplier;
        rb.angularVelocity = Vector3.zero;
    }

    protected override void Update() {
        base.Update();

        transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget.rotation, 0.5f);
    }

    private void LateUpdate() {
        lastPos = transform.position;
        lastAngles = transform.eulerAngles;
    }

    protected override void OnRemoveConnection() {

        Logger.Print("Removing SpringJointConnection from " + transform.name);

        rb.useGravity = true;

        SetVelocity();

        Destroy(joint);
    }

    private void SetVelocity() {
       if (Connector as HandConnector is var handConnector && handConnector != null) {
            rb.velocity = VRInput.Skeleton(handConnector.Hand.HandType).velocity;
            rb.angularVelocity = VRInput.Skeleton(handConnector.Hand.HandType).angularVelocity;
        }
    }

    public static SpringJointConnection Configuration(ItemConnector connector, Transform target, Interactable addTo) {
        Rigidbody targetRB = GetTargetRigidbody(target);

        SpringJointConnection conn = addTo.gameObject.AddComponent<SpringJointConnection>();

        conn.Connector = connector;
        conn.target = targetRB;
        conn.rotationTarget = target;

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

        throw new System.Exception("No target rigidbody found: " + target.name);
    }
}
