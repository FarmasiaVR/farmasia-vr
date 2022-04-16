using UnityEngine;
using System;
using System.Collections.Generic;
public class Fingerprints: Task {

    public enum Conditions { AgarIsTouchedL, AgarIsTouchedR }

    private int donePlates = 0;

    public Fingerprints () : base(TaskType.Fingerprints, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        SubscribeEvent(OnLeftTouch, EventType.FingerprintsGivenL);
        SubscribeEvent(OnRightTouch, EventType.FingerprintsGivenR);
    }

    private void OnLeftTouch(CallbackData data) {
        EnableCondition(Conditions.AgarIsTouchedL);
        donePlates++;
        CompleteTask();
        if(donePlates >= 2){
            twoSameGiven();
        }
    }

    private void OnRightTouch(CallbackData data) {
        EnableCondition(Conditions.AgarIsTouchedR);
        donePlates++;
        CompleteTask();
        if(donePlates >= 2){
            twoSameGiven();
        }        
    }

    public void twoSameGiven(){
        CreateTaskMistake("Annoit saman käden jäljet kahdesti!", 2);
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}

