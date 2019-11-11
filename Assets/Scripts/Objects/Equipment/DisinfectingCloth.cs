using UnityEngine;
using UnityEngine.Assertions;

public class DisinfectingCloth : GeneralItem {

    #region fields
    private GameObject childCollider;
    #endregion

    protected override void Start_GeneralItem() {
        ObjectType = ObjectType.DisinfectingCloth;
        IsClean = true;

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => TouchCloth(collider)));
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
        }
    }
}