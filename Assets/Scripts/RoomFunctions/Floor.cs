using UnityEngine;

public class Floor : MonoBehaviour {

    private void OnTriggerEnter(Collider col) {
        GeneralItem item = GeneralItem.Find(col.gameObject.transform);
        if (item != null) {
            Events.FireEvent(EventType.ItemDroppedOnFloor, CallbackData.Object(item));
            item.IsClean = false;
        }
    }

    private void OnTriggerExit(Collider col) {
        GeneralItem item = GeneralItem.Find(col.gameObject.transform);
        if (item != null) {
            Events.FireEvent(EventType.ItemLiftedOffFloor, CallbackData.Object(item));
        }
    }
}