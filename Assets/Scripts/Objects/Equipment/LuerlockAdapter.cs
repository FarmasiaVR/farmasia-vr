using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    private static float angleLimit = 5;

    private AttachedObject leftObject, rightObject;

    [SerializeField]
    private GameObject leftCollider, rightCollider;

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
        CollisionSubscription.SubscribeToTrigger(leftCollider, ObjectEnterLeft, null, null);
        CollisionSubscription.SubscribeToTrigger(rightCollider, ObjectEnterRight, null, null);
    }

    private void Update() {

        if (VRInput.GetControlDown(Valve.VR.SteamVR_Input_Sources.RightHand, ControlType.Grip)) {
            ReplaceObject(ref leftObject, null);
            ReplaceObject(ref rightObject, null);
        }
    }

    private void ReplaceObject(ref AttachedObject attachedObject, GameObject newObject) {


        Logger.Print("ReplaceObject");
        if (attachedObject.GameObject != null) {

            if (attachedObject.GameObject == newObject) {
                return;
            }

            attachedObject.GameObject.AddComponent<Rigidbody>();
            // attachedObject.Rigidbody.isKinematic = false;
            // attachedObject.Rigidbody.WakeUp();
            attachedObject.GameObject.transform.parent = null;
            attachedObject.GameObject.transform.localScale = attachedObject.Scale;
        }

        attachedObject.GameObject = newObject;
        if (newObject == null) { return; }

        attachedObject.Rigidbody = newObject.GetComponent<Rigidbody>();
        attachedObject.Scale = newObject.transform.localScale;

        Vector3 oldPos = newObject.transform.position;
        Vector3 newScale = new Vector3(
            attachedObject.Scale.x / transform.lossyScale.x,
            attachedObject.Scale.y / transform.lossyScale.y,
            attachedObject.Scale.z / transform.lossyScale.z);

        Destroy(attachedObject.Rigidbody);
        //attachedObject.Rigidbody.isKinematic = true;
        //attachedObject.Rigidbody.Sleep();

        attachedObject.GameObject.transform.parent = transform;
        attachedObject.GameObject.transform.localScale = newScale;
        attachedObject.GameObject.transform.position = oldPos;
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
            ReplaceObject(ref rightObject, GetInteractableObject(collider.transform));
        }
    }
    private void ObjectEnterLeft(Collider collider) {
        Logger.Print("Object entered luerlock adapter left collider");

        if (leftObject.GameObject == null && ConnectingIsAllowed(leftCollider, collider)) {
            // Position Offset here
            ReplaceObject(ref leftObject, GetInteractableObject(collider.transform));
        }
    }

    public bool Attached(bool right) {
        if (right) {
            return rightObject.GameObject != null;
        } else {
            return leftObject.GameObject != null;
        }
    }
    #endregion
}
