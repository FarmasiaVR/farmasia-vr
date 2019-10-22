using UnityEngine;

public class Floor : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        if (item != null) {
            Events.FireEvent(EventType.ItemDroppedOnFloor, CallbackData.Object(item));
            item.IsClean = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        if (item != null) {
            Events.FireEvent(EventType.ItemLiftedOffFloor, CallbackData.Object(item));
        }
    }
}