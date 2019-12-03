using UnityEngine;

public class SyringeCap : GeneralItem {

    #region Fields
    [SerializeField]
    GameObject capHeadTrigger;
    #endregion

    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.SyringeCap;

        Type.On(InteractableType.Interactable, InteractableType.SmallObject);
        CollisionSubscription.SubscribeToTrigger(capHeadTrigger, new TriggerListener().OnEnter(collider => CapHeadCollision(collider)));
    }

    private void CapHeadCollision(Collider other) {
        if (other.name == "SyringeTip" && this.State == InteractState.Grabbed) {
            GameObject foundObject = GetInteractableObject(other.transform);
            GeneralItem item = foundObject?.GetComponent<GeneralItem>();
            Logger.Print(other.gameObject.name + " colliding with syringe cap");
            if (item?.ObjectType == ObjectType.Syringe) {
                Syringe syringe = item as Syringe;
                if (!syringe.HasSyringeCap) AddSyringeCap(syringe);
            } else {
                Logger.Error("Item with syringe tip collided with syringe cap but was not of type Syringe");
            }
        }   
    }

    private void AddSyringeCap(Syringe syringe) {
        syringe.ShowSyringeCap(true);
        syringe.Type.Off(InteractableType.LuerlockAttachable);
        DestroyInteractable();
    }
}
