using System;
using FarmasiaVR.Legacy;

public class CloseAgarPlates : Task {

    public enum Conditions { PlatesClosed }
    private int openedPlates = 0;

    public CloseAgarPlates(TaskType taskType) : base(taskType, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackClosedPlates, EventType.AgarPlateClosed);
        base.SubscribeEvent(TrackOpenedPlates, EventType.PlateOpened);
        base.SubscribeEvent(resetCounter, EventType.ResetCounter);
    }

    private void TrackClosedPlates(CallbackData data) {
        if (openedPlates > 0)
        {
            openedPlates--;
        }
        if (Started) {
            if (openedPlates == 0) {
                EnableCondition(Conditions.PlatesClosed);
                CompleteTask();
            }
        }
    }

    private void TrackOpenedPlates(CallbackData data) {
        openedPlates++;
    }

    public void resetCounter(CallbackData data)
    {
        openedPlates = 0;
    }
}
