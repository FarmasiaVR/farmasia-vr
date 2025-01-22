using System;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class OpenFilterCover : Task {
    public enum Conditions { OpenedFilterCover }
    private CabinetBase laminarCabinet;

    public OpenFilterCover() : base(TaskType.OpenFilterCover, false) {
        SetCheckAll(true);
        
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(FilterCoverOpened, EventType.FilterCoverOpened);
        base.SubscribeEvent(WrongSpotOpened, EventType.WrongSpotOpened);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }
    private void FilterCoverOpened(CallbackData data) {
        var filter = (data.DataObject as FilterInCover);
        CheckIfInsideLaminarCabinet(filter);
        EnableCondition(Conditions.OpenedFilterCover);
        CompleteTask();
    }

    private void CheckIfInsideLaminarCabinet(Interactable interactable) {
        if (laminarCabinet.GetContainedItems().Contains(interactable)) {
            return;
        } else {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpenedFilterCoverOutsideLaminar"), 1);
        }

        if (laminarCabinet.GetContainedItems() == null) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpenedFilterCoverOutsideLaminar"), 1);
        }
    }

    public void WrongSpotOpened(CallbackData data) {
        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpenedFilterCoverPlasticWrongEnd"), 1);
    }
}
