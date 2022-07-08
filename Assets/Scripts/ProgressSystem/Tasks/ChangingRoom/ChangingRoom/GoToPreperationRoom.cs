using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPreperationRoom : Task {

    public enum Conditions { Temp };

    public GoToPreperationRoom() : base(TaskType.GoToPreperationRoom, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(PreviousTasksCompleted, EventType.RoomDoor);
    }

    private void PreviousTasksCompleted(CallbackData data) {
        if ((DoorGoTo)data.DataObject != DoorGoTo.EnterPreparation) return;

        /* if (!wearingShoeCoversAndLabCoat) {
            Popup("Pue ensin suojavarusteet ennen valmistelutilaan siirtymistä.", MsgType.Notify);
            return;
        }
        if (!handsWashed) {
            Popup("Pese ensin kädet ennen valmistelutilaan siirtymistä.", MsgType.Notify);
            return;
        } */

        EnableCondition(Conditions.Temp);
        CompleteTask();
    }

    public override void CompleteTask() {
        base.CompleteTask();
        GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
    }
}
