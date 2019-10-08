using System.Collections.Generic;
using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    public const int RIGHT = 0;
    public const int LEFT = 1;

    private const string luerlockTag = "Luerlock Position";

    private static float angleLimit = 10;
    private static float maxDistance = 0.1f;

    public AttachedObject[] Objects { get; private set; }

    public GameObject[] Colliders { get; private set; }

    public LuerlockConnector Connector { get; private set; }

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
    #endregion

    protected override void Start() {
        base.Start();

        Objects = new AttachedObject[2];
        Colliders = new GameObject[2];

        ObjectType = ObjectType.Luerlock;
        Colliders[LEFT] = transform.Find("Left collider").gameObject;
        Colliders[RIGHT] = transform.Find("Right collider").gameObject;

        SubscribeCollisions();

        Connector = new LuerlockConnector(transform);
    }
    private void SubscribeCollisions() {

        void ObjectEnterLeft(Collider collider) {
            ObjectEnter(collider, LEFT);
        }
        void ObjectEnterRight(Collider collider) {
            ObjectEnter(collider, RIGHT);
        }

        CollisionSubscription.SubscribeToTrigger(Colliders[LEFT], new TriggerListener().OnEnter(ObjectEnterLeft));
        CollisionSubscription.SubscribeToTrigger(Colliders[RIGHT], new TriggerListener().OnEnter(ObjectEnterRight));
    }

    private void OnJointBreak(float breakForce) {
        Logger.Print("Joint force broken: " + breakForce);

        for (int i = 0; i < Connector.Joints.Length; i++) {//have list of joints
            Joint joint = Connector.Joints[i];
            if (breakForce != joint.currentForce.magnitude) {
                continue;
            }
            Connector.ReleaseItem(i);
            break;
        }
    }

    private void Update() {

        if (VRInput.GetControlDown(Valve.VR.SteamVR_Input_Sources.RightHand, ControlType.Grip)) {
            Connector.ConnectItem(null, RIGHT);
            Connector.ConnectItem(null, LEFT);
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

        if (connectingInteractable.Type.IsOff(InteractableType.LuerlockAttachable)) {
            Logger.Print("Interactable is not of type LuerlockAttachable");
            return false;
        }

        Logger.Print("Angle: " + collisionAngle.ToString());
        return true;
    }

    private void ObjectEnter(Collider collider, int side) {
        GameObject intObject = GetInteractableObject(collider.transform);
        if (intObject == null) {
            return;
        }

        if (Objects[side].GameObject == null && ConnectingIsAllowed(Colliders[side], collider)) {
            // Position Offset here
            Connector.ConnectItem(intObject.GetComponent<Interactable>(), side);
        }
    }

    private bool WithinDistance(GameObject collObject, Transform t) {
        return Vector3.Distance(collObject.transform.position, LuerlockPosition(t).position) < maxDistance;
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

    //public bool Attached(int side) {
    //    return Objects[side].GameObject != null;
    //}


    //public int GrabbingSide(Interactable interactable) {
    //    if (interactable.Rigidbody == Objects[RIGHT].Rigidbody) {
    //        return RIGHT;
    //    } else if (interactable.Rigidbody == Objects[LEFT].Rigidbody) {
    //        return LEFT;
    //    }
    //    return -1;
    //}

    //public static KeyValuePair<int, LuerlockAdapter> GrabbingLuerlock(Rigidbody rb) {

    //    LuerlockAdapter[] luerlocks = GameObject.FindObjectsOfType<LuerlockAdapter>();

    //    foreach (LuerlockAdapter luerlock in luerlocks) {
    //        if (rb == luerlock.Objects[RIGHT].Rigidbody) {
    //            return new KeyValuePair<int, LuerlockAdapter>(RIGHT, luerlock);
    //        } else if (rb == luerlock.Objects[LEFT].Rigidbody) {
    //            return new KeyValuePair<int, LuerlockAdapter>(LEFT, luerlock);
    //        }
    //    }

    //    return new KeyValuePair<int, LuerlockAdapter>(-1, null);
    //}
    #endregion
}
