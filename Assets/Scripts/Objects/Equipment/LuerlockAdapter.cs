using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    [SerializeField]
    private GameObject attachedObjectLeft;

    [SerializeField]
    private GameObject attachedObjectRight;

    [SerializeField]
    private GameObject leftCollider;

    [SerializeField]
    private GameObject rightCollider;

    #endregion

    protected override void Start() {
        Logger.Print("Start luerlock adapter");
        ObjectType = ObjectType.Luerlock;
        leftCollider = transform.Find("Left collider").gameObject;
        rightCollider = transform.Find("Right collider").gameObject;
        CollisionSubscription.SubscribeToTrigger(leftCollider, ObjectEnterLeftCollider, null, null);
        CollisionSubscription.SubscribeToTrigger(rightCollider, ObjectEnterRightCollider, null, null);
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

    private void ObjectEnterLeftCollider(Collider collider) {
        float angle = RotationAngle(collider.transform);
        Logger.Print("Object enter left collider with angle: " + angle);
        if (collider.GetComponent<GeneralItem>().ObjectType != ObjectType.Syringe || attachedObjectLeft == collider.gameObject || attachedObjectRight == collider.gameObject ||
            angle < 89.5 || angle > 90.5)
        {
            return;
        }
        Physics.IgnoreCollision(collider, gameObject.GetComponent<Collider>());
        ReplaceObject(ref attachedObjectLeft, collider.gameObject);
    }

    private void ObjectEnterRightCollider(Collider collider) {
        
        float angle = RotationAngle(collider.transform);
        Logger.Print("Object enter right collider with angle: " + angle);
        if (collider.GetComponent<GeneralItem>().ObjectType != ObjectType.Syringe || attachedObjectLeft == collider.gameObject || attachedObjectRight == collider.gameObject ||
            angle < 89.5 || angle > 90.5)
        {
            return;
        }
        Physics.IgnoreCollision(collider, gameObject.GetComponent<Collider>());
        ReplaceObject(ref attachedObjectRight, collider.gameObject);
    }

    private float RotationAngle(Transform collider)
    {
        return Quaternion.Angle(transform.rotation, collider.transform.rotation);
    }

}
