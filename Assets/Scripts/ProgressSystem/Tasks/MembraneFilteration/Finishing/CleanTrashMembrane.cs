using System;

public class CleanTrashMembrane : Task {

    public enum Conditions { TrashCleaned }
    // TRASH
    // - filter parts (filter tank, filter lid, filter base)
    // - 2 serological pipettes
    // - syringe, syringe cap
    // - tweezers
    // - scalpel (sharp)
    //
    // NOT TRASH (make sure these items don't get destroyed)
    // - 3 big bottles and 4 small bottles
    // - 4 agar plates
    // - pump
    // - automatic pipette
    // - small pipette?

    public CleanTrashMembrane() : base(TaskType.CleanTrashMembrane, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackItemsInTrash, EventType.ItemDroppedInTrash);
    }

    private void TrackItemsInTrash(CallbackData data) {
        EnableCondition(Conditions.TrashCleaned);
        CompleteTask();
    }
}
