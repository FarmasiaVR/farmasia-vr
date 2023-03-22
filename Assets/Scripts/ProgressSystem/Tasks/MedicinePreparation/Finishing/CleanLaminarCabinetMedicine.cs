using System;
using FarmasiaVR.Legacy;

public class CleanLaminarCabinetMedicine : Task {

    public enum Conditions { LaminarCabinetCleaned }

    public CleanLaminarCabinetMedicine() : base(TaskType.CleanLaminarCabinetMedicine, true) {
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
