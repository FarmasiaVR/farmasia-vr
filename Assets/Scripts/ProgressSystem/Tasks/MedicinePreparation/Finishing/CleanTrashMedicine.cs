using System;
using FarmasiaVR.Legacy;

public class CleanTrashMedicine : Task {

    public enum Conditions { TrashCleaned }
    private int trashLeft = 3;

    public CleanTrashMedicine() : base(TaskType.CleanTrashMedicine, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackItemsInTrash, EventType.ItemDroppedInTrash);
    }

    private void TrackItemsInTrash(CallbackData data) {
        trashLeft--;
        if (trashLeft <= 0) {
            EnableCondition(Conditions.TrashCleaned);
            CompleteTask();
        }
    }
}
