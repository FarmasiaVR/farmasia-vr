using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandConnector : ItemConnector {


    #region fields
    public Hand Hand { get; private set; }

    public bool IsGrabbed { get; private set; }
    private bool smoothgrab;

    public Rigidbody GrabbedRigidbody { get; private set; }

    private Vector3 grabOffset;
    private Vector3 rotOffset;

    private Joint joint;
    private Joint Joint {
        get {
            if (joint == null) {
                joint = JointConfiguration.AddJoint(Hand.gameObject);
            }
            return joint;
        }
    }

    private ItemConnection connection;
    #endregion

    public HandConnector(Transform obj) : base(obj) {
        Hand = obj.GetComponent<Hand>();
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable, int options) {

        Logger.Print("Connect item");

        if (interactable.State == InteractState.Grabbed) {
            Hand.GrabbingHand(interactable.Rigidbody).Connector.ReleaseItem(0);
        }

        GrabbedRigidbody = interactable.GetComponent<Rigidbody>();

        if (GrabbedRigidbody == null) {
            throw new System.Exception("Interactable had no rigidbody");
        }

        GrabbedRigidbody.GetComponent<Interactable>().State.On(InteractState.Grabbed);
        interactable.Interactors.SetHand(Hand);

        Events.FireEvent(EventType.PickupObject, CallbackData.Object(GrabbedRigidbody.gameObject));
        IsGrabbed = true;
        InitializeOffset();

        if (AllowSmooth(interactable)) {
            SmoothAttachGrabbedObject();
        } else {
            AttachGrabbedObject();
        }
    }

    private void InitializeOffset() {
        grabOffset = GrabbedRigidbody.transform.position - ColliderPosition;
        rotOffset = GrabbedRigidbody.transform.eulerAngles - ColliderEulerAngles;
    }

    private void AttachGrabbedObject() {
        smoothgrab = false;
        Joint.connectedBody = GrabbedRigidbody;
    }
    private void SmoothAttachGrabbedObject() {
        smoothgrab = true;
        connection = SmoothConnection.AddSmoothConnection(this, Hand.Offset, GrabbedRigidbody.gameObject);
    }
    #endregion

    #region Releasing
    public override void ReleaseItem(int options) {
        if (!Hand.IsGrabbed) {
            Logger.Print("Hand is not grabbíng");
        }

        if (Hand.Interactable.State != InteractState.Grabbed) {
            throw new System.Exception("Trying to release ungrabbed item");
        }

        IsGrabbed = false;
        Interactable interactable = Interactable.GetInteractable(GrabbedRigidbody.transform);
        if (smoothgrab) {
            SmoothDeattachGrabbedObject();
        } else {
            DeattachGrabbedObject();
        }
        if (GrabbedRigidbody == null) {
            return;
        }

        smoothgrab = false;

        if (!Hand.Other.IsGrabbed || Hand.Other.Connector.GrabbedRigidbody != GrabbedRigidbody) {
            Interactable.GetInteractable(GrabbedRigidbody.transform).Interactors.SetHand(null);
            GrabbedRigidbody.GetComponent<Interactable>().State.Off(InteractState.Grabbed);
        }

        ItemPlacement.ReleaseSafely(GrabbedRigidbody.gameObject);

        GrabbedRigidbody.velocity = VRInput.Skeleton(Hand.HandType).velocity;
        GrabbedRigidbody.angularVelocity = VRInput.Skeleton(Hand.HandType).angularVelocity;
    }

    private bool AllowSmooth(Interactable interactable) {

        if (interactable.Type != InteractableType.SmallObject) {
            return false;
        }

        if (interactable.State == InteractState.LuerlockAttatch) {

            LuerlockAdapter luerlock = interactable.Interactors.LuerlockPair.Value;

            int count = 0;
            foreach (var obj in luerlock.Objects) {
                if (obj.GameObject != null) {
                    count++;
                }
            }

            if (count > 0) {
                return false;
            }
        }

        return true;
    }

    private void DeattachGrabbedObject() {
        Joint.connectedBody = null;
    }
    private void SmoothDeattachGrabbedObject() {
        MonoBehaviour.Destroy(connection);
        //GrabbedRigidbody.useGravity = true;
    }
    #endregion

    private Vector3 ColliderPosition {
        get {
            return Hand.transform.GetChild(0).transform.position;
        }
    }
    private Vector3 ColliderEulerAngles {
        get {
            return Hand.transform.eulerAngles;
        }
    }
}
