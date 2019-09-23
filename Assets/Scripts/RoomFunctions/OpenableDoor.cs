using UnityEngine;
using UnityEngine.Assertions;

public class OpenableDoor : MonoBehaviour {

    #region fields
    [SerializeField]
    private float maxAngle = 90;

    [SerializeField]
    private float offsetAngle = -45;

    private float startAngle;

    public float Velocity { get; set; }
    private float minVelocity = 0.1f;

    [SerializeField]
    private float friction = 0.9f;

    public bool IsLocked { get; set; } = true;

    private Vector3 lastEulerAngles;

    [SerializeField]
    private Transform handle;
    private float grabLength;
    private float angleOffset;

    private float Angle {
        get {
            return transform.eulerAngles.y;
        }
    }
    #endregion

    private void Start() {
        startAngle = transform.eulerAngles.y + offsetAngle;
        grabLength = float.MaxValue;
        Assert.IsNotNull(handle);
    }

    public void SetByHandPosition(Hand hand) {
        Vector3 handPos = hand.coll.transform.position;

        float handDistance = (handPos - handle.position).magnitude;
        if (handDistance > grabLength) {
            hand.UninteractWithObject();
            ReleaseDoor();
            return;
        }

        lastEulerAngles = transform.eulerAngles;

        Vector3 initialRot = transform.eulerAngles;

        Vector3 direction = transform.position - handPos;
        direction.y = 0;

        Quaternion rawRotation = Quaternion.LookRotation(direction, Vector3.up);
        float angle = rawRotation.eulerAngles.y + angleOffset;

        angle = AngleLock.ClampAngleDeg(angle, startAngle, startAngle + maxAngle);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }

    public void SetAngleOffset(Vector3 handPos) {

        handPos.y = transform.position.y;
        float offsetAdd = Vector3.SignedAngle(handPos - transform.position, -transform.right, Vector3.up);
        float offset = -90 + offsetAdd;

        angleOffset = offset;
    }

    public void ReleaseDoor() {
        Velocity = (transform.eulerAngles.y - lastEulerAngles.y) / Time.deltaTime;
        Logger.PrintVariables("Velocity", Velocity);
        IsLocked = false;
    }

    private void Update() {
        UpdateVelocity();
        RotateDoor();
    }

    private void UpdateVelocity() {
        Velocity *= friction;

        if (Mathf.Abs(Velocity) < minVelocity) {
            Velocity = Velocity > 0 ? minVelocity : -minVelocity;
        }
    }

    private void RotateDoor() {
        if (IsLocked) {
            return;
        }

        Vector3 rotateVector = Vector3.up * Velocity * Time.deltaTime;
        transform.Rotate(rotateVector);

        float fixedAngle = AngleLock.ClampAngleDeg(Angle, startAngle, startAngle + maxAngle);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, fixedAngle, transform.eulerAngles.z);
    }

}
