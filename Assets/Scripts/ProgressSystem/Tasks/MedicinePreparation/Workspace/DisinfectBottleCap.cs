using System;

public class DisinfectBottleCap : Task {

    public enum Conditions { MedicineBottleCapDisinfected };

    public DisinfectBottleCap() : base(TaskType.DisinfectBottleCap, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(BottleDisinfect, EventType.BottleDisinfect);
    }

    private void BottleDisinfect(CallbackData data) {
        EnableCondition(Conditions.MedicineBottleCapDisinfected);
        CompleteTask();
    }
}
