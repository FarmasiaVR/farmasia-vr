using UnityEngine;
using System;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
public class CutFilter: Task {

    public enum Conditions {
        PackagingOpened,
        FilterIsCut
    }
    private CabinetBase laminarCabinet;
    // private bool connected;
    private Pump pump;


    public CutFilter() : base(TaskType.CutFilter, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        SubscribeEvent(FilterCut, EventType.FilterCutted);
        base.SubscribeEvent(ScalpelCoverOpened, EventType.ScalpelCoverOpened);
        base.SubscribeEvent(WrongSpotOpened, EventType.WrongSpotOpened);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(FilterConnected, EventType.AttachFilter);
    }

    private void FilterCut(CallbackData data) {
        if (!package.doneTypes.Contains(TaskType.AssemblePump) && !package.doneTypes.Contains(TaskType.WetFilter) && !package.doneTypes.Contains(TaskType.MedicineToFilter)) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "PrematureFilterCut"), 3);
        }
        if (pump.SlotOccupied) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "InPumpFilterCut"), 1);
        }
        EnableCondition(Conditions.FilterIsCut);
        CompleteTask();
    }
    private void FilterConnected(CallbackData data) {
        pump = data.DataObject as Pump;
        // connected = true;
    }
    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }
    private void ScalpelCoverOpened(CallbackData data) {
        var scalpel = (data.DataObject as Scalpel);
        CheckIfInsideLaminarCabinet(scalpel);
        EnableCondition(Conditions.PackagingOpened);
    }

    private void CheckIfInsideLaminarCabinet(Interactable interactable) {
        if (laminarCabinet.GetContainedItems().Contains(interactable)) {
            return;
        } else {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningFilterCupOutsideLaminarCabinet"), 1);
        }

        if (laminarCabinet.GetContainedItems() == null) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningFilterCupOutsideLaminarCabinet"), 1);
        }
    }

    public void WrongSpotOpened(CallbackData data) {
        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningFilterCupWrongSide"), 1);
    }
}