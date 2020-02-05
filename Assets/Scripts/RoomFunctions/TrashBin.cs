using UnityEngine;

public class TrashBin : MonoBehaviour {

    public enum TrashType {
        Sharp,
        Nonsharp,
        Any
    }

    #region fields
    [SerializeField]
    private TrashType trashType;
    #endregion

    public void EnterTrashbin(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        
        if (item != null) {
            if (trashType == TrashType.Sharp && item.ObjectType != ObjectType.Needle) {
                Logger.Warning("Normal item placed in sharp trash: " + item.ObjectType);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash, CallbackData.Object(item));
            } else if (trashType == TrashType.Nonsharp && item.ObjectType == ObjectType.Needle) {
                Logger.Warning("Sharp item placed in normal trash: " + item.ObjectType);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash, CallbackData.Object(item));
            }

            Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
            item.DestroyInteractable();
        }
    }

    public struct TrashedItem {
        public GeneralItem Item;
        public TrashType Type;
    }
}