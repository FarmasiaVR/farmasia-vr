using System;
using FarmasiaVR.Legacy;

public class CloseBottles : Task {

    public enum Conditions { BottlesClosed }
    private int openedBottles = 0;

    public CloseBottles() : base(TaskType.CloseBottles, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(resetCounters, EventType.ResetCounter);
        base.SubscribeEvent(TrackClosedBottles, EventType.BottleClosed);
        base.SubscribeEvent(TrackOpenedBottles, EventType.BottleOpened);
    }

    private void TrackClosedBottles(CallbackData data) {
        if (openedBottles > 0)
        {
            openedBottles--;
        }
        if (Started) {
            if (openedBottles == 0) {
                EnableCondition(Conditions.BottlesClosed);
                Events.FireEvent(EventType.canCollectTrash, CallbackData.Object(null));
                CompleteTask();

            }
        }
    }

    void resetCounters(CallbackData data)
    {
        openedBottles = 0;
    }
    

    private void TrackOpenedBottles(CallbackData data) {
        openedBottles++;
    }
}
