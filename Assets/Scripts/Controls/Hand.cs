using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    #region fields

    public bool Grabbed { get; set; }

    private Vector3 velocity;
    private Vector3 lastPos;

    private Vector3 angularVelocity;
    private Vector3 lastEulerAngles;

    private HandCollider coll;

    private Rigidbody GrabbedObject { get; set; }
    private Vector3 grabOffset;

    [SerializeField]
    private Hand other;
    #endregion

    private void Start() {
        coll = transform.GetChild(0).GetComponent<HandCollider>();
    }

    public void Grab() {

        Release();

        GrabObject();

        if (GrabbedObject == null) {
            return;
        }

        if (other.Grabbed && other.GrabbedObject.gameObject == GrabbedObject.gameObject) {
            other.Release();
        }

        InitializeOffset();

        Grabbed = true;

        InitVelocities();
    }

    private void InitializeOffset() {
        grabOffset = GrabbedObject.transform.position - ColliderPosition;
    }

    private void InitVelocities() {
        lastPos = transform.position;
        lastEulerAngles = transform.eulerAngles;
    }

    public void Release() {

        Grabbed = false;

        if (GrabbedObject == null) {
            return;
        }

        GrabbedObject.velocity = velocity;
    }

    private Vector3 ColliderPosition {
        get {
            return transform.GetChild(0).transform.position;
        }
    }

    private void GrabObject() {
        GrabbedObject = coll.GetGrabObjet();
    }

    private void Update() {
        UpdateVelocity();

        if (Grabbed) {
            UpdateGrabbedObject();
        }
    }

    private void UpdateGrabbedObject() {
        GrabbedObject.transform.position = ColliderPosition + grabOffset;
    }


    private void UpdateVelocity() {

        Vector3 diff = transform.position - lastPos;
        velocity = diff / Time.deltaTime;
        lastPos = transform.position;

        diff = transform.localEulerAngles - lastEulerAngles;
        angularVelocity = diff / Time.deltaTime;
        lastEulerAngles = transform.eulerAngles;
    }
}
