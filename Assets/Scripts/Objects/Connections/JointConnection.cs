using UnityEngine;

public class JointConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Rigidbody target;
    private Interactable interactable;
    private Joint joint;
    #endregion

    private void Awake() {
        interactable = GetComponent<Interactable>();
    }

    private void SetJoint() {

        joint = JointConfiguration.AddFixedJoint(gameObject);
        joint.connectedBody = target;
    }

    private void OnJointBreak(float force) {
        RemoveConnection();
    }

    protected override void RemoveConnection() {
        Connector.OnReleaseItem();
        Destroy(joint);
    }

    public static JointConnection Configuration(HandConnector connector, Transform target, GameObject addTo) {
        Rigidbody targetRB = target.GetComponent<Rigidbody>();

        if (targetRB == null) {
            throw new System.Exception("Trying to attach joint connection without rigidbody");
        }

        JointConnection conn = addTo.AddComponent<JointConnection>();

        conn.Connector = connector;
        conn.target = targetRB;

        conn.SetJoint();

        return conn;
    }
}
