using UnityEngine;

public class TrashBin : MonoBehaviour {

    #region fields
    [SerializeField]
    private GameObject childCollider;
    #endregion

    private void Start() {
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterTrashbin(collider)));
    }

    private void EnterTrashbin(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        if (item != null) {
            Events.FireEvent(EventType.ItemDroppedInTrash);
            Destroy(other.gameObject);
        }
    }
}