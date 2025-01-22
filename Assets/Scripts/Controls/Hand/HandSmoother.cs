using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSmoother : MonoBehaviour {

    public Hand Hand { get; set; }
    private Transform handOffset;

    [SerializeField]
    private float distanceLimit = 0.02f;

    private float factorMultiplier = 50;

    private bool initMode;
    private Vector3 startPos;
    private float initDistance = 0.02f;

    private void Start() {
        handOffset = Hand.GetOffset();
    }

    private float Distance {
        get {
            return Vector3.Distance(transform.position, handOffset.position);
        }
    }

    public void StartGrab() {
        initMode = true;
        startPos = handOffset.position;
    }

    public void DisableInitMode() {
        initMode = false;
    }

    private void Update() {

        if (initMode) {
            transform.position = startPos;

            if (Distance > initDistance) {
                initMode = false;
            }

            return;
        }

        LerpMove();
        LerpRotate();
    }

    private void LerpRotate() {
        transform.rotation = Quaternion.Lerp(transform.rotation, handOffset.rotation, 0.5f * Time.deltaTime * factorMultiplier);
    }

    private void LerpMove() {
        float factor = Vector3.Distance(transform.position, handOffset.position) / distanceLimit;
        transform.position = Vector3.Lerp(transform.position, handOffset.position, factor * Time.deltaTime * factorMultiplier);
    }
}
