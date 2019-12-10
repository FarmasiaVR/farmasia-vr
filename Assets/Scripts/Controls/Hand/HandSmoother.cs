using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSmoother : MonoBehaviour {

    public Hand Hand { get; set; }
    private Transform target;

    [SerializeField]
    private float force = 10;

    [SerializeField]
    private float distanceLimit = 0.02f;
    [SerializeField]
    private float slowDownFactor = 0.7f;

    private float factorMultiplier = 50;

    private bool initMode;
    private Vector3 startPos;
    private float initDistance = 0.02f;

    private void Start() {
        target = Hand.GetOffset();
    }

    private float Distance {
        get {
            return Vector3.Distance(transform.position, target.position);
        }
    }
    private bool IsClose {
        get {
            return Distance < distanceLimit;
        }
    }

    public void StartGrab() {
        initMode = true;
        startPos = target.position;
    }

    private void FixedUpdate() {
        return;
        if (initMode) {
            return;
        }

        if (IsClose) {
            KinematicAccurateMove();
        }
    }

    private void Update() {

        if (initMode) {
            transform.position = startPos;

            if (Distance > initDistance) {
                initMode = false;
            }

            return;
        }

        if (IsClose) {
            KinematicAccurateMove();
        } else {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
        }

        LerpRotation();
    }

    private void LerpRotation() {
        // FRAMERATE DEPENDENT
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 0.5f);
    }

    private void KinematicAccurateMove() {
        float factor = Vector3.Distance(transform.position, target.position) / distanceLimit;
        transform.position = Vector3.Lerp(transform.position, target.position, factor * Time.deltaTime * factorMultiplier);
    }
}
