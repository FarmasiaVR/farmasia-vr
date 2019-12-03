using System.Collections.Generic;
using UnityEngine;

public abstract class AttachmentConnector : ItemConnector {

    public struct AttachedObject {
        public Interactable Interactable;
        public Vector3 Scale;
        public GameObject GameObject {
            get {
                return Interactable?.gameObject;
            }
        }
        public Rigidbody Rigidbody {
            get {
                return Interactable?.Rigidbody;
            }
        }
    }

    public AttachmentConnector(Transform obj) : base(obj) {
    }

    protected static float angleLimit = 20;
    protected static float maxAttachDistance = 0.2f;

    #region Fields
    public GeneralItem GeneralItem { get; protected set; }
    public GameObject Collider { get; protected set; }
    // public Joint Joint { get; protected set; }
    public bool HasAttachedObject { get => attached.GameObject != null; }
    public Rigidbody AttachedRigidbody { get => attached.Rigidbody; }
    public Interactable AttachedInteractable { get => attached.Interactable; }

    protected AttachedObject attached;

    protected abstract InteractState AttachState { get; }
    #endregion

    public void Subscribe() {
        CollisionSubscription.SubscribeToTrigger(Collider, new TriggerListener().OnEnter(ObjectEnter));
    }


    #region Attaching
    protected void ReplaceObject(GameObject newObject) {
        if (attached.GameObject == newObject) {
            Logger.Print("Attaching same object!");
            return;
        }

        if (attached.GameObject != null) {
            CollisionIgnore.IgnoreCollisions(GeneralItem.transform, attached.GameObject.transform, false);
        }

        if (newObject == null) {
            attached.Interactable = null;
            Logger.Print("New object null!");
            return;
        }

        attached.Scale = newObject.transform.localScale;
        attached.Interactable = newObject.GetComponent<Interactable>();
        SetInteractors();

        // Either override property InteractState AttachState and use State.On(AttachState) or rename LuerlockAttached to Attached
        attached.Interactable.State.On(AttachState);

        CollisionIgnore.IgnoreCollisions(GeneralItem.transform, attached.GameObject.transform, true);

        // Attaching
        SnapObjectPosition();

        // Joint = JointConfiguration.AddJoint(Luerlock.gameObject);
        // Joint.connectedBody = attached.Rigidbody;
        if (attached.GameObject == null) {
            Logger.Error("Attached gameobject null");
        }
        Connection = ItemConnection.AddChildConnection(this, GeneralItem.transform, attached.GameObject);
    }

    #region Type overrides
    protected abstract void SetInteractors();
    protected abstract void SnapObjectPosition();
    protected abstract void AttachEvents(GameObject intObject);
    #endregion


    #endregion

    protected void ObjectEnter(Collider collider) {

        GameObject intObject = Interactable.GetInteractableObject(collider.transform);
        if (intObject == null) {
            return;
        }

        if (attached.GameObject == null && ConnectingIsAllowed(Collider, collider)) {
            // Position Offset here

            Logger.Print("Connecting item");
            ConnectItem(intObject.GetComponent<Interactable>());
            AttachEvents(intObject);
        } else {
            Logger.Print("Not connected");
            Logger.PrintVariables("old obj", attached.GameObject);
        }
    }

    protected bool ConnectingIsAllowed(GameObject adapterCollider, Collider connectingCollider) {
        Interactable connectingInteractable = Interactable.GetInteractable(connectingCollider.transform);
        if (connectingInteractable == null) {
            return false;
        }

        float collisionAngle = Vector3.Angle(adapterCollider.transform.up, connectingInteractable.transform.up);
        if (collisionAngle > angleLimit) {
            Logger.Print("Bad angle: " + collisionAngle.ToString());
            return false;
        }

        if (!IsWithinDistance(adapterCollider, connectingInteractable.transform)) {
            return false;
        }

        if (connectingInteractable.Type.IsOff(InteractableType.LuerlockAttachable)) {
            Logger.Print("Interactable is not of type LuerlockAttachable");
            return false;
        }

        Logger.Print("Angle: " + collisionAngle.ToString());
        return true;
    }

    protected bool IsWithinDistance(GameObject collObject, Transform t) {
        return Vector3.Distance(collObject.transform.position, LuerlockAdapter.LuerlockPosition(t).position) < maxAttachDistance;
    }
}
