using UnityEngine;
using UnityEngine.Assertions;

public class OpenableDoor : MonoBehaviour {

    #region fields
    [SerializeField]
    private bool flipMomentum = false;

    [SerializeField]
    private float maxAngle = 90;

    [SerializeField]
    private float offsetAngle = -45;

    private float startAngle;

    private float AngleSpeed { get; set; }
    private float minAngleSpeed = 0.1f; // Degrees

    [SerializeField]
    private float friction = 0.9f;

    private Vector3 lastEulerAngles;

    [SerializeField]
    private Transform handle;
    private float angleOffset;

    private float doorGrabDistance = 0.25f;
    public bool BreakAtLongDistance { get; set; }
    public float Angle {
        get {
            return transform.eulerAngles.y;
        }
    }
    public bool IsClosed {
        get {
            Debug.Log(Angle);
            return Mathf.Abs(startAngle - offsetAngle - Angle) < 1;
        }
    }
    #endregion

    private void Start() {
        startAngle = transform.eulerAngles.y + offsetAngle;
        Assert.IsNotNull(handle);
    }

    public void SetByHandPosition(Hand hand) {
        Vector3 handPos = hand.ColliderPosition;

        float handDistance = (handPos - handle.position).magnitude;
        if (handDistance > doorGrabDistance && BreakAtLongDistance) {
            hand.Uninteract();
            ReleaseDoor();
            return;
        }

        lastEulerAngles = transform.eulerAngles;

        Vector3 initialRot = transform.eulerAngles;

        Vector3 direction = transform.position - handPos;
        direction.y = 0;
        Quaternion rawRotation = Quaternion.LookRotation(direction, Vector3.up);

        float newAngle = rawRotation.eulerAngles.y + angleOffset;
        float deltaAngle = newAngle - Angle;
        float clampedAngle = AngleLock.ClampAngleDeg(Angle, startAngle, startAngle + maxAngle, deltaAngle);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, clampedAngle, transform.eulerAngles.z);
    }

    public void SetAngleOffset(Vector3 handPos) {

        handPos.y = transform.position.y;
        float offsetAdd = Vector3.SignedAngle(handPos - transform.position, -transform.right, Vector3.up);
        float offset = -90 + offsetAdd;

        angleOffset = offset;
    }

    public void ReleaseDoor() {
        AngleSpeed = (transform.eulerAngles.y - lastEulerAngles.y) / Time.deltaTime;
    }

    private void Update() {
        UpdateVelocity();
        RotateDoor();
    }

    private void UpdateVelocity() {
        AngleSpeed *= friction;
        float speed = Mathf.Abs(AngleSpeed);

        if (speed < minAngleSpeed) {
            AngleSpeed = 0;
        }
    }

    private void RotateDoor() {
        float rotateAngle = AngleSpeed * Time.deltaTime;
        rotateAngle *= flipMomentum ? -1 : 1;

        float clampedAngle = AngleLock.ClampAngleDeg(Angle, startAngle, startAngle + maxAngle, rotateAngle);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, clampedAngle, transform.eulerAngles.z);
    }
}
