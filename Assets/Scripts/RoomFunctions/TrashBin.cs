using UnityEngine;

public class TrashBin : MonoBehaviour {

    #region fields
    [SerializeField]
    protected GameObject childCollider;
    #endregion

    private void Start() {
        Setup(false);
    }

    protected void Setup(bool checkForSharp) {
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterTrashbin(collider, checkForSharp)));
    }

    private void EnterTrashbin(Collider other, bool checkForSharp) {
        GeneralItem item = GeneralItem.Find(other.transform);
        if (item != null) {
            if (item.ObjectType == ObjectType.Needle && !checkForSharp) {
                Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
                item.DestroyInteractable();
            } else {
                Logger.Print("Non sharp item placed in trash");
            }
        }
    }
}