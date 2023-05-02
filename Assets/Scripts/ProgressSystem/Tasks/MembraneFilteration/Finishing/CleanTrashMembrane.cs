using System;
using System.Diagnostics;
using FarmasiaVR.Legacy;
using UnityEngine;

public class CleanTrashMembrane : Task {

    public enum Conditions { TrashCleaned }
    private int trashLeft = 8;

    public CleanTrashMembrane() : base(TaskType.CleanTrashMembrane, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackItemsInTrash, EventType.ItemDroppedInTrash);
    }

    private void TrackItemsInTrash(CallbackData data) {
        trashLeft--;
        UnityEngine.Debug.Log("current trash left: " + trashLeft);
        if (trashLeft <= 0) {
            EnableCondition(Conditions.TrashCleaned);
            CompleteTask();
        }
    }
}
