using UnityEngine;
using System;
using System.Collections.Generic;
public class Fingerprints: Task {

    public enum Conditions {
        AgarIsTouched
    }


    public Fingerprints() : base(TaskType.Fingerprints, true, false) {

        SubscribeEvent((Event) => {
            Logger.Print("Nice touch");
            EnableCondition(Conditions.AgarIsTouched);
            CompleteTask();
            Popup(success, MsgType.Done, Points);
        }, EventType.FingerprintsGiven);
    }
}