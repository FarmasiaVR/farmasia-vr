using UnityEngine;
using System;
using System.Collections.Generic;

class AssemblePump: TaskBase {

    public enum Conditions { PumpAssembled }
    
    private CabinetBase laminarCabinet;
    private bool fail = false;
    private bool firstCheckDone = false;

    public AssemblePump() : base(TaskType.AssemblePump, true, false) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 1;
    }

    public override void Subscribe() {
        base.SubscribeEvent(AttachFilter, EventType.AttachFilter);
    }
    /// <summary>
    /// Check if the filter is connected to pump inside the laminar cabinet
    /// </summary>
    private void AttachFilter(CallbackData data) {
        Pump pump = data.DataObject as Pump;
        PumpFilter filter = pump.GetComponent<PumpFilter>();

        if (filter == null ) {
            return;
        }

        if (laminarCabinet == null) {
            CreateTaskMistake("Filtteri kiinnitettiin liian aikaisin.", 1);
            return;
        } else if (!laminarCabinet.GetContainedItems().Contains(filter)) {
            CreateTaskMistake("Filtteri kiinnitettiin laminaarikaapin ulkopuolella", 1);
            return;
        }
        
        CompleteTask();
    }

    /// <summary>
    /// Check if the medicinewaste pipe is connected to pump
    /// </summary>
    private void AttachPipe(CallbackData data) {
        
    }

 
    private void OnPumpAssemble(CallbackData data) {
        Pump pump = data.DataObject as Pump;
        PumpFilter filter = data.DataObject as PumpFilter;
        bool conn = false;

        if (conn) {
            EnableCondition(Conditions.PumpAssembled);
            CheckMistakes();
            CompleteTask();
        }
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