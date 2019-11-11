using UnityEngine;

public class Floor : MonoBehaviour {

    private void OnCollisionEnter(Collision col) {
        GeneralItem item = GeneralItem.Find(col.gameObject.transform);
        if (item != null) {
            Events.FireEvent(EventType.ItemDroppedOnFloor, CallbackData.Object(item));
            item.IsClean = false;
            UISystem.Instance.CreatePopup("Hae korvaava työväline läpiantokaapista.", MsgType.Notify);
        }
    }

    private void OnTriggerExit(Collider col) {
        GeneralItem item = GeneralItem.Find(col.gameObject.transform);
        if (item != null) {
            Events.FireEvent(EventType.ItemLiftedOffFloor, CallbackData.Object(item));
        }
    }
}