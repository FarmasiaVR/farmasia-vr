using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour {

    private List<ObjectType> normalTrash = new List<ObjectType>() { ObjectType.PumpFilterLid, ObjectType.PumpFilterTank, ObjectType.FilterInCover,
        ObjectType.PipetteContainer, ObjectType.Syringe, ObjectType.SyringeCap, ObjectType.Luerlock, ObjectType.DisinfectingCloth, ObjectType.SterileBag };
    private List<ObjectType> sharpTrash = new List<ObjectType>() { ObjectType.Scalpel, ObjectType.Needle };

    public enum TrashType {
        Normal,
        Sharp
    }

    public TrashType trashType;

    public void OnTrashEnter(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        if (item != null) {
            if (!normalTrash.Contains(item.ObjectType) && !sharpTrash.Contains(item.ObjectType)) {
                Task.CreateGeneralMistake("Esine ei kuulu roskikseen!", 1, true);
                return;
            } else {
                if (item.ObjectType == ObjectType.PipetteContainer && item.gameObject.transform.parent != null) {
                    Task.CreateGeneralMistake("Irroita ensin mittapipetti!", 1, true);
                    return;
                }
                // if (item.ObjectType == ObjectType.SterileBag)
                if (trashType == TrashType.Normal && normalTrash.Contains(item.ObjectType)) {
                    Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
                    G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                }
                if (trashType == TrashType.Sharp && sharpTrash.Contains(item.ObjectType)) {
                    Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
                    G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                }
                if (trashType == TrashType.Sharp && normalTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Normaali esine laitettiin terävien roskikseen", 1, true);
                if (trashType == TrashType.Normal && sharpTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Terävä esine laitettiin normaaliin roskikseen", 1, true);
            }
            PrepareObjectForRemoving(item);
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
}
