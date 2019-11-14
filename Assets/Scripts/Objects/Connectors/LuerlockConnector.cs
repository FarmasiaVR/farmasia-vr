using System.Collections.Generic;
using UnityEngine;

public class LuerlockConnector : ItemConnector {

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

    private static float angleLimit = 10;
    private static float maxDistance = 0.1f;
    private static float breakDistance = 0.2f;

    #region Fields
    public LuerlockAdapter Luerlock { get; private set; }
    public GameObject Collider { get; private set; }
    // public Joint Joint { get; private set; }
    public bool HasAttachedObject { get => attached.GameObject != null; }
    public Rigidbody AttachedRigidbody { get => attached.Rigidbody; }
    public Interactable AttachedInteractable { get => attached.Interactable; }

    private AttachedObject attached;
    public override ItemConnection Connection { get; set; }
    private LuerlockAdapter.Side side;
    #endregion

    public LuerlockConnector(LuerlockAdapter.Side side, LuerlockAdapter luerlock, GameObject collider) : base(luerlock.transform) {
        Luerlock = luerlock;
        attached = new AttachedObject();
        this.side = side;
        this.Collider = collider;
    }

    public void Subscribe() {
        CollisionSubscription.SubscribeToTrigger(Collider, new TriggerListener().OnEnter(ObjectEnter));
    }

    
    #region Attaching
    public override void ConnectItem(Interactable interactable) {
        Logger.Print("Connect item: " + interactable.name);

        bool luerlockGrabbed = Luerlock.State == InteractState.Grabbed;
        Hand luerlockHand = luerlockGrabbed ? Hand.GrabbingHand(Luerlock) : null;

        bool itemGrabbed = interactable.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(interactable) : null;

        // Move to ConnectionHandler?
        // Remove current connections
        if (luerlockGrabbed) {
            // Not necessary but more 'clear' for debugging purposes
            Logger.Print("Luerlock is grabbed, removing grab from luerlock");
            Hand.GrabbingHand(Luerlock).Connector.Connection.Remove();
        }
        if (itemGrabbed) {
            interactable.GetComponent<ItemConnection>().Remove();
        }


        ReplaceObject(interactable?.gameObject);

        // Move to ConnectionHandler?
        // Add new connections
        if (luerlockGrabbed) {
            luerlockHand.InteractWith(Luerlock);
        }
        if (itemGrabbed) {
            itemHand.InteractWith(interactable);
        }
    }

    private void ReplaceObject(GameObject newObject) {
        if (attached.GameObject == newObject) {
            Logger.Print("Attaching same object!");
            return;
        }

        // Disconnect existing joint
        if (attached.GameObject != null) {
            CollisionIgnore.IgnoreCollisions(Luerlock.transform, attached.GameObject.transform, false);
            // MonoBehaviour.Destroy(Joint);
        }

        // Replace with nothing
        if (newObject == null) {
            attached.Interactable = null;
            Logger.Print("New object null!");
            return;
        }

        attached.Scale = newObject.transform.localScale;
        attached.Interactable = newObject.GetComponent<Interactable>();
        attached.Interactable.Interactors.SetLuerlockPair(new KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter>(side, Luerlock));
        attached.Interactable.State.On(InteractState.LuerlockAttached);

        Logger.PrintVariables("luerlock", Luerlock.name);
        Logger.PrintVariables("obj luer: ", attached.Interactable.Interactors.LuerlockPair.Value.gameObject.name);

        CollisionIgnore.IgnoreCollisions(Luerlock.transform, attached.GameObject.transform, true);

        // Attaching
        SnapObjectPosition(Collider);

        // Joint = JointConfiguration.AddJoint(Luerlock.gameObject);
        // Joint.connectedBody = attached.Rigidbody;
        if (attached.GameObject == null) {
            Logger.Error("Attached gameobject null");
        }
        Connection = ItemConnection.AddChildConnection(this, Luerlock.transform, attached.GameObject);
    }

    private void SnapObjectPosition(GameObject collObject) {
        Transform t = attached.GameObject.transform;
        Transform target = LuerlockAdapter.LuerlockPosition(t);

        if (target == null) {
            throw new System.Exception("Luerlock position not found");
        }

        t.up = target.up;

        Vector3 offset = collObject.transform.position - target.position;
        t.position += offset;
    }
    #endregion

    #region Releasing
    public override void OnReleaseItem() {
        Events.FireEvent(EventType.SyringeFromLuerlock, CallbackData.Object(attached.GameObject));
        // MonoBehaviour.Destroy(Joint);
        // MonoBehaviour.Destroy(connection);
        attached.Interactable.Interactors.SetLuerlockPair(new KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter>(side, null));
        attached.Interactable.State.Off(InteractState.LuerlockAttached);
        ReplaceObject(null);
    }
    #endregion

    private void ObjectEnter(Collider collider) {

        GameObject intObject = Interactable.GetInteractableObject(collider.transform);
        if (intObject == null) {
            return;
        }

        if (attached.GameObject == null && ConnectingIsAllowed(Collider, collider)) {
            // Position Offset here

            Logger.Print("Connecting item");
            ConnectItem(intObject.GetComponent<Interactable>());
            Events.FireEvent(EventType.AttachLuerlock, CallbackData.Object(intObject));
            Events.FireEvent(EventType.SyringeToLuerlock, CallbackData.Object(intObject));
        } else {
            Logger.Print("Not connected");
            Logger.PrintVariables("old obj", attached.GameObject);
        }
    }

    private bool ConnectingIsAllowed(GameObject adapterCollider, Collider connectingCollider) {
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

    private bool IsWithinDistance(GameObject collObject, Transform t) {
        return Vector3.Distance(collObject.transform.position, LuerlockAdapter.LuerlockPosition(t).position) < maxDistance;
    }

    public void CheckObjectDistance() {
        if (attached.GameObject == null) {
            return;
        }

        Vector3 luerlockPosition = LuerlockAdapter.LuerlockPosition(attached.GameObject.transform).position;
        Vector3 colliderPosition = Collider.transform.position;
        float distance = Vector3.Distance(luerlockPosition, colliderPosition);

        if (distance > breakDistance) {
            Connection.Remove();
            //Events.FireEvent(EventType.AttachSyringe, CallbackData.Object(intObject));
        }
    }
}
