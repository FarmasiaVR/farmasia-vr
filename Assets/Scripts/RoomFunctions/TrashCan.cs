using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.Legacy;
using UnityEngine.SceneManagement;

public class TrashCan : MonoBehaviour {

    private List<ObjectType> normalTrashMembraneFiltration = new List<ObjectType>() { 
        ObjectType.PumpFilterLid, 
        ObjectType.PumpFilterTank, 
        ObjectType.FilterInCover,
        ObjectType.PipetteContainer, 
        ObjectType.SyringeCap, 
        ObjectType.SterileBag, 
        ObjectType.PumpFilterFilter 
    };
    private List<ObjectType> sharpTrashMembraneFiltration = new List<ObjectType>() { 
        ObjectType.Scalpel,
    };
    private List<ObjectType> medicineTrashMembraneFiltration = new List<ObjectType>() { 
        ObjectType.Syringe 
    };


    private List<ObjectType> normalTrashMedicinePreparation= new List<ObjectType>() {
        ObjectType.Luerlock,
        ObjectType.DisinfectingCloth,
    };
    private List<ObjectType> sharpTrashMedicinePreparation = new List<ObjectType>() {
        ObjectType.Needle
    };
    private List<ObjectType> medicineTrashMedicinePreparation = new List<ObjectType>() {
        ObjectType.Syringe
    };

    public enum TrashType {
        Normal,
        Sharp,
        Medicine
    }

    public TrashType trashType;

    public void OnTrashEnter(Collider other) {
        //GeneralItem item = GeneralItem.Find(other.transform);
        GeneralItem item = other.transform.gameObject.GetComponent<GeneralItem>();
        if (item != null) {
            if (SceneManager.GetActiveScene().name.Contains("MembraneFiltration"))
            {
                checkItemsInTrash(item, normalTrashMembraneFiltration, medicineTrashMembraneFiltration, sharpTrashMembraneFiltration);
            }
            else if (SceneManager.GetActiveScene().name.Contains("MedicinePreparation"))
            {
                checkItemsInTrash(item, normalTrashMedicinePreparation, medicineTrashMedicinePreparation, sharpTrashMedicinePreparation);
            }
        }
    }


    void checkItemsInTrash(GeneralItem item, List<ObjectType> normalTrash, List<ObjectType> medicineTrash, List<ObjectType> sharpTrash)
    {
        if (!normalTrash.Contains(item.ObjectType) && !sharpTrash.Contains(item.ObjectType) && !medicineTrash.Contains(item.ObjectType))
        {
            Task.CreateGeneralMistake("Esine ei kuulu roskikseen!", 1, true);
            return;
        }
        else
        {
            Debug.Log("this item belongs in trash according to logic:" + item.gameObject.name);
            if (item.ObjectType == ObjectType.PipetteContainer && item.gameObject.transform.parent != null)
            {
                Task.CreateGeneralMistake("Irroita ensin mittapipetti!", 1, true);
                return;
            }
            // if (item.ObjectType == ObjectType.SterileBag)
            if (trashType == TrashType.Medicine && medicineTrash.Contains(item.ObjectType))
            {
                Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
                G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                PrepareObjectForRemoving(item);
                item.DestroyInteractable();
            }
            if (trashType == TrashType.Normal && normalTrash.Contains(item.ObjectType))
            {
                Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
                G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                PrepareObjectForRemoving(item);
                item.DestroyInteractable();
            }
            if (trashType == TrashType.Sharp && sharpTrash.Contains(item.ObjectType))
            {
                Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
                G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                PrepareObjectForRemoving(item);
                item.DestroyInteractable();
            }
            if (trashType == TrashType.Normal && medicineTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Lääkejäte esine laitettiin normaaliin roskikseen", 1, true);
            if (trashType == TrashType.Sharp && medicineTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Lääkejäte esine laitettiin terävien roskikseen", 1, true);
            if (trashType == TrashType.Sharp && normalTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Normaali esine laitettiin terävien roskikseen", 1, true);
            if (trashType == TrashType.Medicine && normalTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Normaali esine laitettiin lääkejätteen roskikseen", 1, true);
            if (trashType == TrashType.Normal && sharpTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Terävä esine laitettiin normaaliin roskikseen", 1, true);
            if (trashType == TrashType.Medicine && sharpTrash.Contains(item.ObjectType)) Task.CreateGeneralMistake("Terävä esine laitettiin lääkejätteen roskikseen", 1, true);
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
