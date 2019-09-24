using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    [SerializeField]
    private GameObject attachedObjectLeft;

    [SerializeField]
    private GameObject attachedObjectRight;

    #endregion

    protected new void Start() {
        ObjectType = ObjectType.Luerlock;
        CollisionSubscription.SubscribeToTrigger(transform.Find("Left collider").gameObject, ObjectEnterLeftCollider, null, null);
        CollisionSubscription.SubscribeToTrigger(transform.Find("Right collider").gameObject, ObjectEnterRightCollider, null, null);
    }

    private void ReplaceObject(ref GameObject attachedObject, GameObject newObject) {
        if (attachedObject != null) {
            attachedObject.GetComponent<Rigidbody>().isKinematic = false;
            attachedObject.transform.parent = null;
            attachedObject = null;
        }
        attachedObject = newObject;
        attachedObject.GetComponent<Rigidbody>().isKinematic = true;
        attachedObject.transform.SetParent(this.transform);
    }

    private void ObjectEnterLeftCollider(Collider collider) {
        Logger.Print("Object enter left collider");
        AttachSyringe(collider.gameObject);

        if (collider.GetComponent<GeneralItem>().ObjectType != ObjectType.Syringe ||
           attachedObjectLeft == collider.gameObject) {
            return;
        }

        ReplaceObject(ref attachedObjectLeft, collider.gameObject);
    }

    private void ObjectEnterRightCollider(Collider collider) {

        if (collider.GetComponent<GeneralItem>().ObjectType != ObjectType.Syringe ||
           attachedObjectRight == collider.gameObject) {
            return;
        }

        ReplaceObject(ref attachedObjectRight, collider.gameObject);

        Logger.Print("Object enter right collider");
        AttachSyringe(collider.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject collisionObject = collision.gameObject;

        if (collisionObject == attachedObjectLeft || collisionObject == attachedObjectRight) {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
            return;
        }

        Logger.Print("Luerlock adapter hit something");
    }

    private void AttachSyringe(GameObject syringe) {


        if (attachedObjectLeft == null) {
            attachedObjectLeft = syringe;
        } else if (attachedObjectRight == null) {
            attachedObjectRight = syringe;
        }
    }
}
