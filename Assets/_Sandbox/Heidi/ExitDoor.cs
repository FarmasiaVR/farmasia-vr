using System;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

    #region Fields
    private int itemsInTrashCount;
    private int itemsOnFloorCount;
    #endregion

    private void Start() {
        GameObject floor = GameObject.FindWithTag("Floor");
        GameObject trash = GameObject.FindWithTag("TrashBin");
        CollisionSubscription.SubscribeToTrigger(trash, new TriggerListener().OnEnter(c => itemsInTrashCount++));
        CollisionSubscription.SubscribeToTrigger(floor, new TriggerListener()
                .OnEnter(c => itemsOnFloorCount++)
                .OnExit(c => itemsOnFloorCount--)
        );
    }

    public void CheckExitPermission() {
        if (G.Instance.Progress.IsCurrentPackage(PackageName.CleanUp)) {
            bool allItemsFromFloorToTrash = itemsOnFloorCount == itemsInTrashCount;
            //Events.FireEvent(EventType.CleanUp, CallbackData.Boolean(allItemsFromFloorToTrash));
            //finish task Finish
            //quit game
        } else {
            UISystem.Instance.CreatePopup("Peli on vielä käynnissä. Ovi avautuu vasta kaikkien vaiheiden suorituksen jälkeen.", MsgType.Notify);
        }
    }
}