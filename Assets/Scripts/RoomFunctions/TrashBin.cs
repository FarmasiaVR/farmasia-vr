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
                TaskBase.CreateGeneralMistake("Normaali esine laitettiin terävien roskikseen", 1, true);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash);
            } else if (trashType == TrashType.Nonsharp && item.ObjectType == ObjectType.Needle) {
                TaskBase.CreateGeneralMistake("Neula laitettiin normaaliin roskikseen", 1, true);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash);
            }

            //PrepareObjectForRemoving(item);

            if (item.ObjectType == ObjectType.Luerlock) {
                Logger.Print("Trash bin luerlock count: " + ((LuerlockAdapter)item).AttachedInteractables.Count);
            }

            PrepareObjectForRemoving(item);

            Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
            item.DestroyInteractable();
        }
    }

    private void PrepareObjectForRemoving(GeneralItem item) {

        if (item.IsGrabbed) {
            item.Interactors.Hand.Connector.Connection.Remove();
        }

        if (item.ObjectType == ObjectType.Needle) {
            ((Needle)item).ReleaseItem();
        } else if (item.ObjectType == ObjectType.Luerlock) {
            ((LuerlockAdapter)item).ReleaseItems();
        }

        if (item.IsAttached) {
            if (item.State == InteractState.LuerlockAttached) {
                item.Interactors.LuerlockPair.Value.GetConnector(item.Interactors.LuerlockPair.Key).Connection.Remove();
            } else if (item.State == InteractState.NeedleAttached) {
                item.Interactors.Needle.Connector.Connection.Remove();
            }
        }
    }

    public struct TrashedItem {
        public GeneralItem Item;
        public TrashType Type;
    }
}