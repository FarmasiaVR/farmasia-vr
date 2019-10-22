using UnityEngine;

public class TrashBin : MonoBehaviour {

    #region fields
    public int droppedItemsInArea { get; private set; }

    [SerializeField]
    private GameObject childCollider;
    #endregion

    private void Start() {
        droppedItemsInArea = 0;
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterTrashbin(collider)));
    }

    private void EnterTrashbin(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }

        droppedItemsInArea++;
        if (!foundObject.GetComponent<GeneralItem>().IsClean) {
            if (G.Instance.Progress.currentPackage.name != "Clean up") {
                UISystem.Instance.CreatePopup("Dropped item was put to trash before time", MessageType.Notify);
            }
        }
        Destroy(foundObject);
    }
}