using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    [SerializeField]
    private GameObject attachedObjectLeft;
    public GameObject AttachedObjectLeft {
        get => attachedObjectLeft;
        set {
            if (attachedObjectLeft != null)
            {
                attachedObjectLeft.GetComponent<Rigidbody>().isKinematic = false;
                attachedObjectLeft.transform.parent = null;
                attachedObjectLeft = null;
            }
            attachedObjectLeft = value;

            attachedObjectLeft.GetComponent<Rigidbody>().isKinematic = true;
            attachedObjectLeft.transform.SetParent(this.transform);
            attachedObjectLeft.transform.localPosition = Vector3.zero;
        }
    }

    [SerializeField]
    private GameObject attachedObjectRight;
    public GameObject AttachedObjectRight {
        get => attachedObjectRight;
        set {
            if (attachedObjectRight != null)
            {
                attachedObjectRight.GetComponent<Rigidbody>().isKinematic = false;
                attachedObjectRight.transform.parent = null;
                attachedObjectRight = null;
            }
            attachedObjectRight = value;
            attachedObjectRight.GetComponent<Rigidbody>().isKinematic = true;
            attachedObjectRight.transform.SetParent(this.transform);
        }
    }
    #endregion

    protected new void Start() {
        ObjectType = ObjectType.Luerlock;
        CollisionSubscription.SubscribeToTrigger(transform.Find("Left collider").gameObject, ObjectEnterLeftCollider, null, null);
        CollisionSubscription.SubscribeToTrigger(transform.Find("Right collider").gameObject, ObjectEnterRightCollider, null, null);
    }

    private void ObjectEnterLeftCollider(Collider collider)
    {
        Logger.Print("Object enter left collider");
        AttachSyringe(collider.gameObject);
    }

    private void ObjectEnterRightCollider(Collider collider)
    {
        Logger.Print("Object enter right collider");
        AttachSyringe(collider.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collisionObject == AttachedObjectLeft || collisionObject == AttachedObjectRight)
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
            return;
        }

        Logger.Print("Luerlock adapter hit something");
    }

    private void AttachSyringe(GameObject syringe)
    {
        if (syringe.GetComponent<GeneralItem>().ObjectType != ObjectType.Syringe ||
            AttachedObjectLeft == syringe || AttachedObjectRight == syringe)
        {
            return;
        }

        if (AttachedObjectLeft == null)
        {
            AttachedObjectLeft = syringe;
        } else if (attachedObjectRight == null)
        {
            AttachedObjectRight = syringe;
        }
    }
}
