using UnityEngine;

public class SyringeCap : GeneralItem {

    #region Fields
    [SerializeField]
    GameObject capHeadTrigger;
    #endregion

    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.SyringeCap;

        Type.On(InteractableType.Interactable);
        CollisionSubscription.SubscribeToTrigger(capHeadTrigger, new TriggerListener().OnEnter(collider => CapHeadCollision(collider)));
    }

    private void CapHeadCollision(Collider other) {
        if (other.name == "SyringeTip" && this.State == InteractState.Grabbed) {
            GameObject foundObject = GetInteractableObject(other.transform);
            GeneralItem item = foundObject?.GetComponent<GeneralItem>();
            Logger.Print(string.Format("{0} colliding with syringe cap", other.gameObject.name));
            if (item?.ObjectType == ObjectType.Syringe) {
                Syringe syringe = item as Syringe;
                if (!syringe.HasSyringeCap) {
                    AddSyringeCap(syringe);
                    DestroyInteractable();
                }
            } else {
                Logger.Error("Item with syringe tip collided with syringe cap but was not of type Syringe");
            }
        }   
    }

    public static void AddSyringeCap(Syringe syringe) {
        if (syringe.State == InteractState.LuerlockAttached || syringe.State == InteractState.NeedleAttached) {
            Logger.Warning("Cannot add cap to syringe if it is attached to something");
            return;
        }

        syringe.ShowSyringeCap(true);
        syringe.Type.Off(InteractableType.Attachable);
    }
}
