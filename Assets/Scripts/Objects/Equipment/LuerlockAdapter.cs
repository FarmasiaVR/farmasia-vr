using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    [SerializeField]
    private GameObject attachedObject1;
    public GameObject AttachedObject1 {
        get => attachedObject1;
        set {
            if (attachedObject1 != null)
            {
                attachedObject1.transform.parent = null;
                attachedObject1 = null;
            }
            attachedObject1 = value;

            attachedObject1.GetComponent<Rigidbody>().isKinematic = true;
            attachedObject1.transform.SetParent(this.transform);
            attachedObject1.transform.localPosition = Vector3.zero;
        }
    }

    [SerializeField]
    private GameObject attachedObject2;
    public GameObject AttachedObject2 {
        get => attachedObject2;
        set {
            if (attachedObject2 != null)
            {
                attachedObject2.transform.parent = null;
                attachedObject2 = null;
            }
            attachedObject2 = value;
            attachedObject2.GetComponent<Rigidbody>().isKinematic = true;
            attachedObject2.transform.SetParent(this.transform);
        }
    }
    #endregion

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Luerlock;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collisionObject == AttachedObject1 || collisionObject == AttachedObject2)
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
            return;
        }

        if (collisionObject.GetComponent<GeneralItem>().ObjectType == ObjectType.Syringe)
        {
            AttachSyringe(collisionObject);
        }

        Logger.Print("Luerlock adapter hit something");
    }

    private void AttachSyringe(GameObject syringe)
    {
        if (syringe.GetComponent<GeneralItem>().ObjectType != ObjectType.Syringe ||
            AttachedObject1 == syringe || AttachedObject2 == syringe)
        {
            return;
        }

        if (AttachedObject1 == null)
        {
            AttachedObject1 = syringe;
        } else if (attachedObject2 == null)
        {
            AttachedObject2 = syringe;
        }
    }
}
