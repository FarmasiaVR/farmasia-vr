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
                Logger.Print("Trash bin luerlock count: " + ((LuerlockAdapter)item).AttachedInteractables.Count.ToString());
            }

            PrepareObjectForRemoving(item);

            Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
            item.DestroyInteractable();
        }
    }

    private void PrepareObjectForRemoving(GeneralItem interactable) {

        if (interactable.IsGrabbed) {
            interactable.Interactors.Hand.Connector.Connection.Remove();
        }

        if (interactable.ObjectType == ObjectType.Needle) {
            ((Needle)interactable).ReleaseItem();
        } else if (interactable.ObjectType == ObjectType.Luerlock) {
            ((LuerlockAdapter)interactable).ReleaseItems();
        }

        if (interactable.IsAttached) {
            if (interactable.State == InteractState.LuerlockAttached) {
                interactable.Interactors.LuerlockPair.Value.GetConnector(interactable.Interactors.LuerlockPair.Key).Connection.Remove();
            } else if (interactable.State == InteractState.ConnectableAttached) {
                interactable.Interactors.ConnectableItem.Connector.Connection.Remove();
            }
        }
    }

    public struct TrashedItem {
        public GeneralItem Item;
        public TrashType Type;
    }
}