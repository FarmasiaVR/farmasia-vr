using System;

public class FinishChangingRoom : Task {

    public enum Conditions { };

    public FinishChangingRoom() : base(TaskType.FinishChangingRoom, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        // base.SubscribeEvent(DoStuffWhenThatEventHappens, EventType.YourEventType);
    }

    private void DoStuffWhenThatEventHappens(CallbackData data) {

    }
}
