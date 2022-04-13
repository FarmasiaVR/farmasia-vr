using UnityEngine;
using System;
using System.Collections.Generic;
public class Fingerprints: Task {

    public enum Conditions { AgarIsTouchedL, AgarIsTouchedR }

    public Fingerprints () : base(TaskType.Fingerprints, true, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
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

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}

