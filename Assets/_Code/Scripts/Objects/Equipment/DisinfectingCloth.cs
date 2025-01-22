using UnityEngine;

public class DisinfectingCloth : GeneralItem {

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.DisinfectingCloth;
        Type.On(InteractableType.Interactable);
    }

    protected override void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);
        GameObject foundObject = GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        if (item.ObjectType == ObjectType.Medicine) {
            Bottle bottle = item as Bottle;
            bottle.Contamination = ContaminateState.Clean;   
        }
    }
}
