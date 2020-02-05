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
                TaskBase.CreateGeneralMistake("Normal item placed in sharp trash", 1, true);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash);
            } else if (trashType == TrashType.Nonsharp && item.ObjectType == ObjectType.Needle) {
                TaskBase.CreateGeneralMistake("Sharp item placed in normal trash", 1, true);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash);
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