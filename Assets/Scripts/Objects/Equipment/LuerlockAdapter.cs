using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    private static float angleLimit = 25;

    private GameObject leftObject, rightObject;

    [SerializeField]
    private GameObject leftCollider, rightCollider;
    #endregion

    protected override void Start() {
        // base.Start();
        ObjectType = ObjectType.Luerlock;
        leftCollider = transform.Find("Left collider").gameObject;
        rightCollider = transform.Find("Right collider").gameObject;
        CollisionSubscription.SubscribeToTrigger(leftCollider, ObjectEnterLeft, null, null);
        CollisionSubscription.SubscribeToTrigger(rightCollider, ObjectEnterRight, null, null);
    }

    private void ReplaceObject(ref GameObject attachedObject, GameObject newObject) {

        Logger.Print("ReplaceObject");
        if (attachedObject != null) {
            attachedObject.GetComponent<Rigidbody>().isKinematic = false;
            attachedObject.transform.parent = null;
        }
        attachedObject = newObject;
        if (newObject == null) { return; }

        attachedObject.GetComponent<Rigidbody>().isKinematic = true;
        attachedObject.transform.SetParent(transform, true);
        attachedObject.transform.localPosition = new Vector3(0, 0, attachedObject.transform.localPosition.z);
    }

    #region Attaching
    private bool ConnectingIsAllowed(GameObject adapterCollider, Collider connectingCollider) {
        float collisionAngle = Quaternion.Angle(adapterCollider.transform.rotation, connectingCollider.transform.rotation);
        if (collisionAngle > 90 + angleLimit || collisionAngle < 90 - angleLimit) {
            Logger.Print("Bad angle: " + collisionAngle.ToString());
            return false;
        }
        Interactable connectingInteractable = connectingCollider.GetComponent<Interactable>();
        if (connectingInteractable.Types.IsOff(InteractableType.LuerlockAttachable)) {
            Logger.Print("Interactable is not of type LuerlockAttachable");
            return false;
        }
        return true;
    }

    private void ObjectEnterRight(Collider collider) {
        Logger.Print("Object entered luerlock adapter right collider");

        if (rightObject == null && ConnectingIsAllowed(rightCollider, collider)) {
            // Position Offset here
            ReplaceObject(ref rightObject, collider.gameObject);
        }
    }
    private void ObjectEnterLeft(Collider collider) {
        Logger.Print("Object entered luerlock adapter left collider");

        if (leftObject == null && ConnectingIsAllowed(rightCollider, collider)) {
            // Position Offset here
            ReplaceObject(ref leftObject, collider.gameObject);
        }
    }

    public bool Attached(bool right) {
        if (right) {
            return rightObject != null;
        } else {
            return leftObject != null;
        }
    }
    #endregion
}
