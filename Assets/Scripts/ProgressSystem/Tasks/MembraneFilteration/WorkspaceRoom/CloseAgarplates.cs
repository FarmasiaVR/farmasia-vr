using UnityEngine;
using System;
using System.Collections.Generic;

public class CloseAgarplates : Task {

    #region Fields
    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions { PlatesClosed }
    private int openedPlates = 0;
    #endregion

    public CloseAgarplates() : base(TaskType.CloseAgarplates, true, true) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackClosedPlates, EventType.AgarPlateClosed);
        base.SubscribeEvent(TrackOpenedPlates, EventType.PlateOpened);
    }

    private void TrackClosedPlates(CallbackData data) {
        openedPlates--;
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

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}