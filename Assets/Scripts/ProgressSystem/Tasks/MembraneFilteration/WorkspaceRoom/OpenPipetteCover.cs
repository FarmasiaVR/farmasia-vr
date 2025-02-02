using System;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class OpenPipetteCover : Task {
    public enum Conditions { OpenedPipetteCover }
    private CabinetBase laminarCabinet;

    public OpenPipetteCover() : base(TaskType.OpenPipetteCover, false) {
        SetCheckAll(true);

        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(PipetteCoverOpened, EventType.PipetteCoverOpened);
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
    private void PipetteCoverOpened(CallbackData data) {
        var pipetteContainer = (data.DataObject as PipetteContainer);
        CheckIfInsideLaminarCabinet(pipetteContainer);
        EnableCondition(Conditions.OpenedPipetteCover);
        CompleteTask();
    }

    private void CheckIfInsideLaminarCabinet(Interactable interactable) {
        if (laminarCabinet.GetContainedItems().Contains(interactable)) {
            return;
        }
        else {
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