using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTools : TaskData {
    int[] instanceIDs;

    public SelectTools() {
        instanceIDs = new int[10];
        SubscribeEvents();
    }

    public void SubscribeEvents() {
        Events.SubscribeToEvent(PickupObject, EventType.PickupObject);
    }

    public void UnsubscribeEvents() {
        Events.UnsubscribeFromEvent(PickupObject, EventType.PickupObject);
    }

    
    public void FinishTask() {
        UnsubscribeEvents();
    }

    public string GetDescription() {
        return "Valitse sopiva määrä välineitä.";
    }

    public string GetHint() {
        throw new System.NotImplementedException();
    }

    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        

    }



    public void NextTask() {
        ProgressManager.Instance.AddTask(TaskType.SelectMedicine);
    }

    public bool CheckPreviousTaskCompletion() {
        throw new System.NotImplementedException();
    }
}
