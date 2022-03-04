using UnityEngine;
using System;
using System.Collections.Generic;

class AssemblePump: TaskBase {

    public enum Conditions { PumpAssembled }
    
    private CabinetBase laminarCabinet;
    // private bool fail = false;
    // private bool firstCheckDone = false;

    public AssemblePump() : base(TaskType.AssemblePump, true, false) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 1;
        Subscribe();
    }

    public override void Subscribe() {
        base.SubscribeEvent(AttachFilter, EventType.AttachFilter);
    }
    /// <summary>
    /// Check if the filter is connected to pump inside the laminar cabinet
    /// </summary>
    private void AttachFilter(CallbackData data) {
        Logger.Print("Started to attach filter to "+ data.DataObject);
        Pump pump = data.DataObject as Pump;

    /*    if (laminarCabinet == null) {
            CreateTaskMistake("Filtteri kiinnitettiin liian aikaisin.", 1);
            return;
        } else if (!laminarCabinet.GetContainedItems().Contains(pump)) {
            CreateTaskMistake("Filtteri kiinnitettiin laminaarikaapin ulkopuolella", 1);
            return;
        }
    */
        EnableCondition(Conditions.PumpAssembled);
        CompleteTask();
    }

    /// <summary>
    /// Check if the medicinewaste pipe is connected to pump
    /// </summary>
    private void AttachPipe(CallbackData data) {
        Logger.Print("Started to attach pipe to "+ data.DataObject);

        // EnableCondition(Conditions.PipeAttached);
        // CompleteTask();
    }

    private void CheckMistakes() {
        
    }

    protected override void OnTaskComplete() {

    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (IsCompleted()) {
            Popup("Hienoa, pumppu on koossa", MsgType.Done);
        }
    }

    public override string GetDescription() {
        return "Kokoa pumppu!";
    }

    public override string GetHint() {
        return "Kiinnitä filtteri pumppuun.";
    }
}