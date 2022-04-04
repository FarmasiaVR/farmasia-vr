using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMembrane : Task {

    private CabinetBase laminarCabinet;
 
    ///  <summary>
    ///  Constructor for Finish task.
    ///  Is not removed when finished and requires previous task completion.
    ///  </summary>
    public FinishMembrane() : base(TaskType.FinishMembrane, false, true) {
        Subscribe();
    }

    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }
    public override void StartTask() {
        if (!Started) {
            FinishTask();
        }
        base.StartTask();
    }
    public override async void FinishTask() {
        await System.Threading.Tasks.Task.Delay(1000);
        CompleteTask();
        base.FinishTask();
    }
}
