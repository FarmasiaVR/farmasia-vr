using UnityEngine;
using UnityEngine.Assertions;

public class DisinfectingCloth : GeneralItem {

    #region fields
    [SerializeField]
    private GameObject collider;
    #endregion

    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.DisinfectingCloth;
        IsClean = true;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);
        CollisionSubscription.SubscribeToTrigger(collider, new TriggerListener().OnEnter(collider => TouchCloth(collider)));
    }

    private void TouchCloth(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        if (item.ObjectType == ObjectType.Bottle && this.IsClean) {
            MedicineBottle bottle = item as MedicineBottle;
            bottle.IsClean = true;
            UISystem.Instance.CreatePopup("Lääkepullon korkki puhdistettu.", MsgType.Notify);
        }
    }
}