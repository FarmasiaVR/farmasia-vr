using System;
using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.Legacy;

public class GoToPreperationRoom : Task {

    public enum Conditions { PreviousTasksCompleted };
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.WearShoeCoversAndLabCoat, TaskType.WashHandsInChangingRoom };

    public GoToPreperationRoom() : base(TaskType.GoToPreperationRoom, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackCompletedTasks, EventType.RoomDoor);
    }

    private void TrackCompletedTasks(CallbackData data) {
        if ((DoorGoTo)data.DataObject != DoorGoTo.EnterPreparation) return;
        if (!IsPreviousTasksCompleted(requiredTasks)) {
            Popup("Suorita ensin tarvittavat toimenpiteet ennen valmistelutilaan siirtymistä.", MsgType.Notify);
            return;
        }
        EnableCondition(Conditions.PreviousTasksCompleted);
        CompleteTask();
        GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
    }
}
