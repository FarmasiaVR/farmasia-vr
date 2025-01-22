using System;
using FarmasiaVR.Legacy;

public class WashGlasses : Task {

    public enum Conditions { GlassesCleaned };

    public WashGlasses() : base(TaskType.WashGlasses, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(OnGlassesWet, EventType.CleaningGlasses);
    }

    private void OnGlassesWet(CallbackData data) {
        EnableCondition(Conditions.GlassesCleaned);
        CompleteTask();
    }
}
