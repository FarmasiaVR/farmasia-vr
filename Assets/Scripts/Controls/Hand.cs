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

    public Interactable Interactable { get; private set; }
    public Rigidbody GrabbedRigidbody { get; private set; }
    private Vector3 grabOffset;
    private Vector3 rotOffset;

    [SerializeField]
    private Hand other;
    #endregion

    private void Start() {
        coll = transform.GetChild(0).GetComponent<HandCollider>();
    }

    public void InteractWithObject() {

        if (Grabbed) {
            Release();
            return;
        }

        Interactable = coll.GetGrab();

        if (Interactable == null) {
            return;
        }

        if (Interactable.Type == GrabType.Grabbable) {
            Grab();
        } else if (Interactable.Type == GrabType.Interactable) {
            Interactable.Interact(this);
        } else if (Interactable.Type == GrabType.GrabbableAndInteractable) {
            throw new System.Exception("not implemented");
        }
    }
    public void UninteractWithObject() {

        if (Interactable != null) {
            Interactable.Uninteract(this);
            Interactable = null;
            return;
        }

        // Necessary???
        Interactable = coll.GetGrab();

        if (Interactable == null) {
            return;
        }

        if (Interactable.Type == GrabType.Interactable) {
            Interactable.Uninteract(this);
            Interactable = null;
        } else if (Interactable.Type == GrabType.GrabbableAndInteractable) {
            throw new System.Exception("not implemented");
        } else if (Interactable.Type == GrabType.DraggableAndInteractable) {

        }
    }

    private void Grab() {

        GrabObject();

        if (GrabbedRigidbody == null) {
            return;
        }

        if (other.Grabbed && other.GrabbedRigidbody.gameObject == GrabbedRigidbody.gameObject) {
            other.Release();
        }

        InitializeOffset();

        Grabbed = true;

        InitVelocities();
    }

    private void InitializeOffset() {
        grabOffset = GrabbedRigidbody.transform.position - ColliderPosition;
        rotOffset = GrabbedRigidbody.transform.eulerAngles - ColliderEulerAngles;
    }

    private void InitVelocities() {
        lastPos = transform.position;
        lastEulerAngles = transform.eulerAngles;
    }

    public void Release() {

        Grabbed = false;

        if (GrabbedRigidbody == null) {
            return;
        }

        GrabbedRigidbody.velocity = velocity;
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
        GrabbedRigidbody = Interactable.GetComponent<Rigidbody>();
    }

    private void Update() {
        UpdateVelocity();

        if (Grabbed) {
            UpdateGrabbedObject();
        }
    }

    // Alternative: set Rigidbody to kinematic, might cause bugs though
    private void UpdateGrabbedObject() {
        GrabbedRigidbody.velocity = Vector3.zero;
        GrabbedRigidbody.transform.position = ColliderPosition + grabOffset;

        GrabbedRigidbody.angularVelocity = Vector3.zero;
        GrabbedRigidbody.transform.eulerAngles = ColliderEulerAngles + rotOffset;
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
