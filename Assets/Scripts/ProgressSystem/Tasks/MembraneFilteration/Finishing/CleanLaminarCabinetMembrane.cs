using System;
using FarmasiaVR.Legacy;

public class CleanLaminarCabinetMembrane : Task {

    public enum Conditions { LaminarCabinetCleaned }

    public CleanLaminarCabinetMembrane() : base(TaskType.CleanLaminarCabinetMembrane, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackWallsCleaned, EventType.CleaningBottleSprayed);
    }

    private void TrackWallsCleaned(CallbackData data) {
        if (Started) {
            EnableCondition(Conditions.LaminarCabinetCleaned);
            CompleteTask();
        }
    }
}
