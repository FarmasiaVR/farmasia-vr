using System;
using System.Collections.Generic;

public class EmptyLaminarCabinetMembrane : Task {

    public enum Conditions { LaminarCabinetEmpty };
    private CabinetBase laminarCabinet;

    public EmptyLaminarCabinetMembrane() : base(TaskType.EmptyLaminarCabinetMembrane, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CheckLaminarCabinet, EventType.CheckLaminarCabinetItems);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void CheckLaminarCabinet(CallbackData data) {
        if (laminarCabinet == null) {
            Logger.Error("Laminar cabinet was null in EmptyLaminarCabinetMembrane. Items not placed for reference.");
            return;
        }
        List<Interactable> objects = laminarCabinet.GetContainedItems();
        if (objects.Count == 0) {
            EnableCondition(Conditions.LaminarCabinetEmpty);
            CompleteTask();
            return;
        }
    }
}
