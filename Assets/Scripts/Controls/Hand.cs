using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region fields

    public bool Grabbed { get; set; }

    private VRHandControls controls;
    private FixedJoint joint;
    private FixedJoint Joint {
        get {
            if (joint == null) {
                AddJoint();
            }
            return joint;
        }
    }

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
        controls = GetComponent<VRHandControls>();

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
        // GrabbedRigidbody.transform.position = ColliderPosition + grabOffset;

        GrabbedRigidbody.angularVelocity = Vector3.zero;
        // GrabbedRigidbody.transform.eulerAngles = ColliderEulerAngles + rotOffset;
    }


    private void UpdateVelocity() {

        Vector3 diff = transform.position - lastPos;
        velocity = diff / Time.deltaTime;
        lastPos = transform.position;

        diff = transform.localEulerAngles - lastEulerAngles;
        angularVelocity = diff / Time.deltaTime;
        lastEulerAngles = transform.eulerAngles;
    }

    private void AddJoint() {
        joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = 1000;
        joint.breakTorque = 1000;
    }


    #region Interaction
    public void InteractWithObject() {

        if (Grabbed) {
            Release();
            return;
        }

        Interactable = coll.GetGrab();

        if (Interactable == null) {
            return;
        }

        if (Interactable.Type == InteractableType.Grabbable) {
            Grab();
        } else if (Interactable.Type == InteractableType.Interactable) {
            Interactable.Interact(this);
        } else if (Interactable.Type == InteractableType.GrabbableAndInteractable) {
            Grab();
        }
    }
    public void UninteractWithObject() {

        if (Grabbed) {
            if (VRControlSettings.HoldToGrab) {
                Release();
            }
        } else if (Interactable != null) {
            Interactable.Uninteract(this);
            Interactable = null;
        }
    }

    public void GrabInteract() {
        if (!Grabbed) {
            return;
        }

        Interactable = coll.GetGrab();

        if (Interactable.Type == InteractableType.GrabbableAndInteractable) {
            Interactable.Interact(this);
        }
    }

    public void GrabUninteract() {
        if (!Grabbed) {
            return;
        }

        Interactable = coll.GetGrab();

        if (Interactable.Type == InteractableType.GrabbableAndInteractable) {
            Interactable.Uninteract(this);
        }
    }
    #endregion



    private void Grab() {

        GrabObject();

        if (GrabbedRigidbody == null) {
            return;
        }

        // Fix later
        GrabbedRigidbody.transform.position = transform.GetChild(0).position;

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedRigidbody.gameObject));

        if (other.Grabbed && other.GrabbedRigidbody.gameObject == GrabbedRigidbody.gameObject) {
            other.Release();
        }

        InitializeOffset();

        Grabbed = true;

        InitVelocities();

        AttachGrabbedObject();
    }
    

    private void OnJointBreak(float breakForce) {
        Debug.Log("Joint force broken: " + breakForce);
        Release();
    }

    public void Release() {

        Grabbed = false;

        DeattachGrabbedObject();

        if (GrabbedRigidbody == null) {
            return;
        }

        GrabbedRigidbody.velocity = controls.Skeleton.velocity;
        GrabbedRigidbody.angularVelocity = controls.Skeleton.angularVelocity;
    }

    #region Helper functions
    private void AttachGrabbedObject() {
        Joint.connectedBody = GrabbedRigidbody;
    }
    private void DeattachGrabbedObject() {
        Joint.connectedBody = null;
    }

    private void InitializeOffset() {
        grabOffset = GrabbedRigidbody.transform.position - ColliderPosition;
        rotOffset = GrabbedRigidbody.transform.eulerAngles - ColliderEulerAngles;

        print("Grab offset: " + grabOffset);
    }

    private void InitVelocities() {
        lastPos = transform.position;
        lastEulerAngles = transform.eulerAngles;
    }

    private Vector3 ColliderPosition {
        get {
            // return transform.position;
            return transform.GetChild(0).transform.position;
        }
    }
    private Vector3 ColliderEulerAngles {
        get {
            return transform.eulerAngles;
            return transform.GetChild(0).transform.eulerAngles;
        }
    }

    private void GrabObject() {
        GrabbedRigidbody = Interactable.GetComponent<Rigidbody>();
    }
    #endregion

   
}
