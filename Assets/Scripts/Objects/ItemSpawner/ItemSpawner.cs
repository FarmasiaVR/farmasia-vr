using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    [SerializeField]
    GameObject copy;
    GameObject currentObject;

    private void Start() {
        Events.SubscribeToEvent(Copy, EventType.ItemDroppedOnFloor);
        Events.SubscribeToEvent(Copy, EventType.ItemDroppedInTrash);
        if (copy != null) {
            currentObject = copy;
        }
    }

    public void SetCopyObject(GameObject gob) {
        copy = gob;
        currentObject = gob;
    }

    public void Copy(CallbackData data) {
        GeneralItem item = (GeneralItem) data.DataObject;
        if (item.gameObject == currentObject || currentObject == null) {
            currentObject = Instantiate(copy, transform.position, transform.rotation);
        }
    }
}
