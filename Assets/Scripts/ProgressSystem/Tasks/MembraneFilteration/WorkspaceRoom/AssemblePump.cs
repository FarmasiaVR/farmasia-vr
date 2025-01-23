using UnityEngine;
using System;
using System.Collections.Generic;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

class AssemblePump: Task {

    public enum Conditions { FilterPackagingOpened, FilterAttached, PipeAttached }
    
    private CabinetBase laminarCabinet;
    // private bool fail = false;
    // private bool firstCheckDone = false;

    public AssemblePump() : base(TaskType.AssemblePump, false) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        
    }

    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(AttachFilter, EventType.AttachFilter);
        base.SubscribeEvent(AttachPipe, EventType.AttachPipe);
        base.SubscribeEvent(FilterCoverOpened, EventType.FilterCoverOpened);
        base.SubscribeEvent(WrongSpotOpened, EventType.WrongSpotOpened); // 2023 the filter in cover does not have a wrong spot to open so this is never activated.
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }
    /// <summary>
    /// Check if the filter is connected to pump inside the laminar cabinet
    /// </summary>
    private void AttachFilter(CallbackData data) {
        Logger.Print("Started to attach filter to "+ data.DataObject);
        Pump pump = data.DataObject as Pump;

        if (laminarCabinet == null) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "EarlyPumpAssembly"), 1);
            return;
        } else if (!laminarCabinet.GetContainedItems().Contains(pump)) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OutsidePumpAssembly"), 1);
            return;
        }
    
        EnableCondition(Conditions.FilterAttached);
        CompleteTask();
    }

    private void FilterCoverOpened(CallbackData data) {
        Debug.Log("Filter cover opened!");
        var filter = (data.DataObject as FilterInCover);
        CheckIfInsideLaminarCabinet(filter);
        EnableCondition(Conditions.FilterPackagingOpened);
    }

    private void CheckIfInsideLaminarCabinet(Interactable interactable) {
        if (laminarCabinet.GetContainedItems().Contains(interactable)) {
            return;
        } else {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningFilterPumpAssembly"), 1);
        }

        if (laminarCabinet.GetContainedItems() == null) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningFilterPumpAssembly"), 1);
        }
    }

    public void WrongSpotOpened(CallbackData data) {
        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongSideOpeningPumpAssembly"), 1);
    }

    /// <summary>
    /// Check if the medicinewaste pipe is connected to pump
    /// </summary>
    private void AttachPipe(CallbackData data) {
        Logger.Print("Started to attach pipe to "+ data.DataObject);

        EnableCondition(Conditions.PipeAttached);
        CompleteTask();
    }
}