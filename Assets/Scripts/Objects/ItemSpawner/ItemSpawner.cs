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

    private void Awake() {
        Logger.Print("Created item spawnder");
    }

    public void SetCopyObject(GameObject gob) {
        Logger.Print("Initialized item spawner");
        copy = gob;
        currentObject = gob;
    }

    public void Copy(CallbackData data) {
        if (copy == null) {
            return;
        }
        GeneralItem item = (GeneralItem) data.DataObject;
        if (item.gameObject == currentObject || currentObject == null) {
            currentObject = Instantiate(copy, transform.position, transform.rotation);
            currentObject.GetComponent<GeneralItem>().IsClean = true;
        }
    }
}
