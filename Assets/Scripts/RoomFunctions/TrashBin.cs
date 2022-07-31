using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour {

    private List<ObjectType> normalTrash = new List<ObjectType>() { ObjectType.PumpFilterLid, ObjectType.PumpFilterTank, ObjectType.FilterInCover,
        ObjectType.PipetteContainer, ObjectType.Syringe, ObjectType.SyringeCap, ObjectType.Tweezers };
    private List<ObjectType> sharpTrash = new List<ObjectType>() { ObjectType.Scalpel };

    public enum TrashType {
        Normal,
        Sharp
    }

    public TrashType trashType;

    public void EnterTrashbin(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        if (item != null) {
            if (trashType == TrashType.Normal && normalTrash.Contains(item.ObjectType)) Logger.Print("CORRECT: NORAML TRASH IN NORMAL");
            if (trashType == TrashType.Sharp && normalTrash.Contains(item.ObjectType)) Logger.Print("WRONG: NORMAL TRASH IN SHARP");
            if (trashType == TrashType.Normal && sharpTrash.Contains(item.ObjectType)) Logger.Print("WRONG: SHARP TRASH IN NORMAL");
            if (trashType == TrashType.Sharp && sharpTrash.Contains(item.ObjectType)) Logger.Print("CORRECT: SHARP TRASH IN SHARP");
            Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
            /*
            if (trashType == TrashType.Normal && item.ObjectType == ObjectType.Needle) {
                Task.CreateGeneralMistake("Neula laitettiin normaaliin roskikseen", 1, true);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash);
            } else if (trashType == TrashType.Sharp && item.ObjectType != ObjectType.Needle) {
                Task.CreateGeneralMistake("Normaali esine laitettiin terävien roskikseen", 1, true);
                Events.FireEvent(EventType.ItemDroppedInWrongTrash);
            }

            if (item.ObjectType == ObjectType.Luerlock) {
                Logger.Print("Trash bin luerlock count: " + ((LuerlockAdapter)item).AttachedInteractables.Count.ToString());
            }

            PrepareObjectForRemoving(item);

            Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
            item.DestroyInteractable();
            */
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