using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    private const string luerlockTag = "Luerlock Position";

    private static float angleLimit = 5;
    private static float maxDistance = 0.025f;

    private AttachedObject leftObject, rightObject;

    [SerializeField]
    private GameObject leftCollider, rightCollider;

    private Rigidbody rb;

    private struct AttachedObject {
        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Vector3 Scale;
    }
    #endregion

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Luerlock;
        leftCollider = transform.Find("Left collider").gameObject;
        rightCollider = transform.Find("Right collider").gameObject;
        CollisionSubscription.SubscribeToTrigger(leftCollider, new TriggerListener().OnEnter(ObjectEnterLeft));
        CollisionSubscription.SubscribeToTrigger(rightCollider, new TriggerListener().OnEnter(ObjectEnterRight));

        rb = GetComponent<Rigidbody>();
    }

    private void Update() {

        if (VRInput.GetControlDown(Valve.VR.SteamVR_Input_Sources.RightHand, ControlType.Grip)) {
            ReplaceObject(ref leftObject, null, false);
            ReplaceObject(ref rightObject, null, true);
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

        if (rightObject.GameObject == null && ConnectingIsAllowed(rightCollider, collider)) {
            // Position Offset here
            ReplaceObject(ref rightObject, GetInteractableObject(collider.transform), true);
        }
    }
    private void ObjectEnterLeft(Collider collider) {
        Logger.Print("Object entered luerlock adapter left collider");

        if (leftObject.GameObject == null && ConnectingIsAllowed(leftCollider, collider)) {
            // Position Offset here
            ReplaceObject(ref leftObject, GetInteractableObject(collider.transform), false);
        }
    }

    private void ReplaceObject(ref AttachedObject attachedObject, GameObject newObject, bool right) {

        GameObject colliderT = right ? rightCollider : leftCollider;

        Logger.Print("ReplaceObject");
        if (attachedObject.GameObject != null) {

            if (attachedObject.GameObject == newObject) {
                return;
            }

            Physics.IgnoreCollision(GetComponent<Collider>(), attachedObject.GameObject.GetComponent<Collider>(), false);

            // attachedObject.GameObject.AddComponent<Rigidbody>();
            attachedObject.Rigidbody.isKinematic = false;
            // attachedObject.Rigidbody.WakeUp();
            attachedObject.GameObject.transform.parent = null;
            attachedObject.GameObject.transform.localScale = attachedObject.Scale;
        }

        attachedObject.GameObject = newObject;
        if (newObject == null) { return; }

        attachedObject.Rigidbody = newObject.GetComponent<Rigidbody>();
        attachedObject.Scale = newObject.transform.localScale;

        // FIX
        if (Hand.GrabbingHand(rb) != null) {
            Hand.GrabbingHand(attachedObject.Rigidbody)?.Release();
        } else {

            // ERRORS WILL COME HERE

        }

        Physics.IgnoreCollision(GetComponent<Collider>(), attachedObject.GameObject.GetComponent<Collider>(), true);

        Vector3 newScale = new Vector3(
            attachedObject.Scale.x / transform.lossyScale.x,
            attachedObject.Scale.y / transform.lossyScale.y,
            attachedObject.Scale.z / transform.lossyScale.z);

        // Destroy(attachedObject.Rigidbody);
        attachedObject.Rigidbody.isKinematic = true;
        //attachedObject.Rigidbody.Sleep();

        attachedObject.GameObject.transform.parent = transform;
        attachedObject.GameObject.transform.localScale = newScale;
        attachedObject.GameObject.transform.up = colliderT.transform.up;
        SetLuerlockPosition(colliderT, attachedObject.GameObject.transform);
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

    public bool Attached(bool right) {
        if (right) {
            return rightObject.GameObject != null;
        } else {
            return leftObject.GameObject != null;
        }
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
    #endregion
}
