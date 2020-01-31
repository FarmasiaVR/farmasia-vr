using UnityEngine;
using UnityEngine.Assertions;

public class DisinfectingCloth : GeneralItem {

    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.DisinfectingCloth;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);
    }

    protected override void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);

        GameObject foundObject = GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        if (item.ObjectType == ObjectType.Bottle && this.IsClean) {
            MedicineBottle bottle = item as MedicineBottle;
            if (!bottle.IsClean) {
                bottle.Contamination = ContaminateState.Clean;
                UISystem.Instance.CreatePopup("Lääkepullon korkki puhdistettu.", MsgType.Done);
                Events.FireEvent(EventType.BottleDisinfect, CallbackData.Object(bottle));
            }
        }
    }
}