using System;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

    private void Start() {
    }

    public void CheckExitPermission() {
        GameObject floor = GameObject.FindWithTag("Floor");
        GameObject trashBin = GameObject.FindWithTag("TrashBin");
        if (String.Equals(G.Instance.Progress.currentPackage.name, "Clean up")) {
            bool allItemsFromFloorToTrash = (floor.GetComponent<Floor>().droppedItems == trashBin.GetComponent<TrashBin>().droppedItemsInArea);
            Events.FireEvent(EventType.CleanUp, CallbackData.Boolean(allItemsFromFloorToTrash));
            //finish task Finish
            //quit game
        } else {
            UISystem.Instance.CreatePopup("Peli on vielä käynnissä. Ovi avautuu vasta kaikkien vaiheiden suorituksen jälkeen.", MessageType.Notify);
        }
    }
}