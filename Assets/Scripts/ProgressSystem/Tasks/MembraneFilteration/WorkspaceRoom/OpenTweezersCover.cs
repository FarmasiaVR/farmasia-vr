using System;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class OpenTweezersCover : Task {
    public enum Conditions { OpenedTweezersCover }
    private CabinetBase laminarCabinet;

    public OpenTweezersCover() : base(TaskType.OpenTweezersCover, false) {
        SetCheckAll(true);
        
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TweezersCoverOpened, EventType.TweezersCoverOpened);
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
    private void TweezersCoverOpened(CallbackData data) {
        var tweezers = (data.DataObject as Tweezers);
        CheckIfInsideLaminarCabinet(tweezers);
        EnableCondition(Conditions.OpenedTweezersCover);
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
        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpenedSharpObjectFromWrongEnd"), 1);
    }
}
