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
        Logger.Print("Start luerlock adapter");
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
    private void ObjectEnterRight(Collider collider) {

        Interactable interatable = collider.GetComponent<Interactable>();

        float angle = Quaternion.Angle(rightCollider.transform.rotation, collider.transform.rotation);

        if (angle > angleLimit || Attached(true) || interatable.Types.IsOff(InteractableType.LuerlockAttachable)) {
            return;
        }

        // Position Offset Here

        ReplaceObject(ref rightObject, collider.gameObject);
    }
    private void ObjectEnterLeft(Collider collider) {

        Interactable interatable = collider.GetComponent<Interactable>();

        float angle = Quaternion.Angle(leftCollider.transform.rotation, collider.transform.rotation);

        if (angle > angleLimit || Attached(false) || interatable.Types.IsOff(InteractableType.LuerlockAttachable)) {
            return;
        }

        // Position Offset Here

        ReplaceObject(ref leftObject, collider.gameObject);
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
