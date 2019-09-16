using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour {

    #region fields

    [SerializeField]
    private float maxAngle = 90;
    private float startAngle;

    public float Velocity { get; set; }
    private float minVelocity = 0.1f;

    [SerializeField]
    private float friction = 0.9f;

    public bool Locked { get; set; } = true;

    private Vector3 lastEulerAngles;

    #endregion

    private void Start() {
        startAngle = transform.eulerAngles.y;
    }

    public void SetByHandPosition(Vector3 handPos) {
        Vector3 newLastEulerAngles = transform.eulerAngles;

        Vector3 initialRot = transform.eulerAngles;
        
        Vector3 direction = transform.position - handPos;
        direction.y = 0;

        float offset = -90;
        Quaternion rawRotation = Quaternion.LookRotation(direction, Vector3.up);
        float angle = rawRotation.eulerAngles.y + offset;

        angle = AngleLock.ClampAngleDeg(angle, startAngle, startAngle + maxAngle);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }

    private Vector3 FixAngles(Vector3 angles, Vector3 last) {

        if (lastEulerAngles.y > 180) {

            if (angles.y <= 180) {
                angles.y = startAngle + maxAngle;
                return angles;
            }

        } else {

            if (angles.y > 180) {
                angles.y = startAngle;
                return angles;
            }
        }

        lastEulerAngles = last;
        angles.y = Mathf.Clamp(angles.y, startAngle, startAngle + maxAngle);
        return angles;
    }

    public void ReleaseDoor() {

        Velocity = (transform.eulerAngles.y - lastEulerAngles.y) / Time.deltaTime;

        Logger.PrintVariables("Velocity", Velocity);

        Locked = false;
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

        if (Locked) {
            return;
        }

        Vector3 rotateVector = Vector3.up * Velocity * Time.deltaTime;

        transform.Rotate(rotateVector);

        float fixedAngle = AngleLock.ClampAngleDeg(Angle, startAngle, startAngle + maxAngle);

        if (Angle != fixedAngle) {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, fixedAngle, transform.eulerAngles.z);
        }
    }

    private float Angle {
        get {
            return transform.eulerAngles.y;
        }
    }
}
