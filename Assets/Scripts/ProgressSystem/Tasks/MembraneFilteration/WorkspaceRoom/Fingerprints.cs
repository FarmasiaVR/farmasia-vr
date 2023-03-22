using System;
using FarmasiaVR.Legacy;

public class Fingerprints : Task {

    public enum Conditions { AgarIsTouchedL, AgarIsTouchedR }

    public Fingerprints() : base(TaskType.Fingerprints, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        SubscribeEvent(OnLeftTouch, EventType.FingerprintsGivenL);
        SubscribeEvent(OnRightTouch, EventType.FingerprintsGivenR);
    }

    private void OnLeftTouch(CallbackData data) {
        EnableCondition(Conditions.AgarIsTouchedL);
        CompleteTask();
    }

    private void OnRightTouch(CallbackData data) {
        EnableCondition(Conditions.AgarIsTouchedR);
        CompleteTask();
    }
}
