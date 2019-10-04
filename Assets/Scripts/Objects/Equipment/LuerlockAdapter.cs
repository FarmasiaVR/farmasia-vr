using System.Collections.Generic;
using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    public const int RIGHT = 0;
    public const int LEFT = 1;

    private const string luerlockTag = "Luerlock Position";

    private static float angleLimit = 10;
    private static float maxDistance = 0.1f;

    private AttachedObject[] objects;

    private GameObject[] colliders;

    public ItemConnector Connector { get; private set; }

    private struct AttachedObject {
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
    #endregion

    protected override void Start() {
        base.Start();

        objects = new AttachedObject[2];
        colliders = new GameObject[2];

        ObjectType = ObjectType.Luerlock;
        colliders[LEFT] = transform.Find("Left collider").gameObject;
        colliders[RIGHT] = transform.Find("Right collider").gameObject;
        CollisionSubscription.SubscribeToTrigger(colliders[LEFT], new TriggerListener().OnEnter(ObjectEnterLeft));
        CollisionSubscription.SubscribeToTrigger(colliders[RIGHT], new TriggerListener().OnEnter(ObjectEnterRight));

        Connector = new ItemConnector();
    }

    private void Update() {

        if (VRInput.GetControlDown(Valve.VR.SteamVR_Input_Sources.RightHand, ControlType.Grip)) {
            ReplaceObject(LEFT, null);
            ReplaceObject(RIGHT, null);
        }
    }



    #region Attaching
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

        if (!WithinDistance(adapterCollider, connectingInteractable.transform)) {
            return false;
        }

        if (connectingInteractable.Types.IsOff(InteractableType.LuerlockAttachable)) {
            Logger.Print("Interactable is not of type LuerlockAttachable");
            return false;
        }

        Logger.Print("Angle: " + collisionAngle.ToString());
        return true;
    }

    private void ObjectEnterRight(Collider collider) {
        Logger.Print("Object entered luerlock adapter right collider");

        if (objects[RIGHT].GameObject == null && ConnectingIsAllowed(colliders[RIGHT], collider)) {
            // Position Offset here
            ReplaceObject(RIGHT, GetInteractableObject(collider.transform));
        }
    }
    private void ObjectEnterLeft(Collider collider) {
        Logger.Print("Object entered luerlock adapter left collider");

        if (objects[LEFT].GameObject == null && ConnectingIsAllowed(colliders[LEFT], collider)) {
            // Position Offset here
            ReplaceObject(LEFT, GetInteractableObject(collider.transform));
        }
    }

    private void ReplaceObject(int side, GameObject newObject) {

        GameObject colliderT = colliders[side];

        AttachedObject obj = objects[side];

        Logger.Print("ReplaceObject");
        if (obj.GameObject != null) {

            if (obj.GameObject == newObject) {
                return;
            }

            IgnoreCollisions(transform, obj.GameObject.transform, false);

            // attachedObject.GameObject.AddComponent<Rigidbody>();
            obj.Rigidbody.isKinematic = false;
            // attachedObject.Rigidbody.WakeUp();
            obj.GameObject.transform.parent = null;
            obj.GameObject.transform.localScale = obj.Scale;
        }

        obj.Interactable = newObject.GetComponent<Interactable>();
        if (newObject == null) { return; }

        obj.Scale = newObject.transform.localScale;

        // FIX
        if (Hand.GrabbingHand(Rigidbody) != null) {
            Hand.GrabbingHand(obj.Rigidbody)?.Connector.ReleaseItemFromHand();
        } else {

            // ERRORS WILL COME HERE

        }

        IgnoreCollisions(transform, obj.GameObject.transform, true);

        Vector3 newScale = new Vector3(
            obj.Scale.x / transform.lossyScale.x,
            obj.Scale.y / transform.lossyScale.y,
            obj.Scale.z / transform.lossyScale.z);

        // Destroy(attachedObject.Rigidbody);
        obj.Rigidbody.isKinematic = true;
        //attachedObject.Rigidbody.Sleep();

        obj.GameObject.transform.parent = transform;
        obj.GameObject.transform.localScale = newScale;
        obj.GameObject.transform.up = colliderT.transform.up;
        SetLuerlockPosition(colliderT, obj.GameObject.transform);

        objects[side] = obj;
    }

    private bool WithinDistance(GameObject collObject, Transform t) {
        return Vector3.Distance(collObject.transform.position, LuerlockPosition(t).position) < maxDistance;
    }
    private void SetLuerlockPosition(GameObject collObject, Transform t) {

        Transform target = LuerlockPosition(t);

        if (target == null) {
            throw new System.Exception("Luerlock position not found");
        }

        Vector3 offset = collObject.transform.position - target.position;
        t.position += offset;
    }

    public bool Attached(int side) {
        return objects[side].GameObject != null;
    }

    public static Transform LuerlockPosition(Transform t) {

        if (t.tag == luerlockTag) {
            return t;
        }

        foreach (Transform c in t) {

            Transform l = LuerlockPosition(c);

            if (l != null) {
                return l;
            }
        }

        return null;
    }

    private static void IgnoreCollisions(Transform a, Transform b, bool ignore) {

        Collider coll = a.GetComponent<Collider>();

        if (coll != null) {
            IgnoreCollisionsCollider(coll, b, ignore);
        }

        foreach (Transform child in a) {
            IgnoreCollisions(child, b, ignore);
        }
    }
    private static void IgnoreCollisionsCollider(Collider a, Transform b, bool ignore) {

        Collider coll = b.GetComponent<Collider>();

        if (coll != null) {
            Physics.IgnoreCollision(a, coll, ignore);
            foreach (Transform child in b) {
                IgnoreCollisionsCollider(a, child, ignore);
            }
        }
    }

    public int GrabbingSide(Interactable interactable) {
        if (interactable.Rigidbody == objects[RIGHT].Rigidbody) {
            return RIGHT;
        } else if (interactable.Rigidbody == objects[LEFT].Rigidbody) {
            return LEFT;
        }
        return -1;
    }

    public static KeyValuePair<int, LuerlockAdapter> GrabbingLuerlock(Rigidbody rb) {

        LuerlockAdapter[] luerlocks = GameObject.FindObjectsOfType<LuerlockAdapter>();

        foreach (LuerlockAdapter luerlock in luerlocks) {
            if (rb == luerlock.objects[RIGHT].Rigidbody) {
                return new KeyValuePair<int, LuerlockAdapter>(RIGHT, luerlock);
            } else if (rb == luerlock.objects[LEFT].Rigidbody) {
                return new KeyValuePair<int, LuerlockAdapter>(LEFT, luerlock);
            }
        }

        return new KeyValuePair<int, LuerlockAdapter>(-1, null);
    }
    #endregion
}
