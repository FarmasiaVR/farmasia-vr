using System;

public class WashGlasses : Task {

    public enum Conditions { GlassesCleaned };

    public WashGlasses() : base(TaskType.WashGlasses, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(DoStuff, EventType.CleaningGlasses);
    }

    // Temporary
    private void DoStuff(CallbackData data) {
        EnableCondition(Conditions.GlassesCleaned);
        CompleteTask();
    }
}
