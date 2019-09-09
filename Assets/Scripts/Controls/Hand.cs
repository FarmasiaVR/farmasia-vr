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
    private Vector3 rotOffset;

    [SerializeField]
    private Hand other;
    #endregion

    private void Start() {
        coll = transform.GetChild(0).GetComponent<HandCollider>();
    }

    public void Grab() {

        if (Grabbed) {
            Release();
            return;
        }

        print("Grab");

        GrabObject();

        if (GrabbedObject == null) {
            Logger.PrintVariables("grabbed", Grabbed, "grabbed object", GrabbedObject);
            return;
        }

        Logger.PrintVariables("grabbed", Grabbed, "grabbed object", GrabbedObject);

        if (other.Grabbed && other.GrabbedObject.gameObject == GrabbedObject.gameObject) {
            other.Release();
        }

        Debug.Log("Grab succesful");

        InitializeOffset();

        Grabbed = true;

        InitVelocities();
    }

    private void InitializeOffset() {
        grabOffset = GrabbedObject.transform.position - ColliderPosition;
        rotOffset = GrabbedObject.transform.eulerAngles - ColliderEulerAngles;
    }

    private void InitVelocities() {
        lastPos = transform.position;
        lastEulerAngles = transform.eulerAngles;
    }

    public void Release() {

        print("Release");

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
    private Vector3 ColliderEulerAngles {
        get {
            return transform.GetChild(0).transform.eulerAngles;
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

    // Alternative: set Rigidbody to kinematic, might cause bugs though
    private void UpdateGrabbedObject() {
        GrabbedObject.velocity = Vector3.zero;
        GrabbedObject.transform.position = ColliderPosition + grabOffset;

        GrabbedObject.angularVelocity = Vector3.zero;
        GrabbedObject.transform.eulerAngles = ColliderEulerAngles + rotOffset;
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
