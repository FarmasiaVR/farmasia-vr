using System;

public class WashGlasses : Task {

    public enum Conditions { };

    public WashGlasses() : base(TaskType.WashGlasses, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        // base.SubscribeEvent(DoStuffWhenThatEventHappens, EventType.YourEventType);
    }

    private void DoStuffWhenThatEventHappens(CallbackData data) {

    }
}
