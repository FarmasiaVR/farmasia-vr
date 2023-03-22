using System;
using FarmasiaVR.Legacy;


public class FilterHalvesToBottles : Task {
    public enum Conditions { HavlesInBottles, OpenedTweezersCover }

    private int filterHalvesInSoycaseine = 0;
    private int filterHalvesInTioglycolate = 0;
    private CabinetBase laminarCabinet;

    public FilterHalvesToBottles() : base(TaskType.FilterHalvesToBottles, true) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        SubscribeEvent(HalfToBottle, EventType.FilterHalfEnteredBottle);
        SubscribeEvent(TouchedFilter, EventType.TouchedFilterWithHand);
        base.SubscribeEvent(TweezersCoverOpened, EventType.TweezersCoverOpened);
        base.SubscribeEvent(WrongSpotOpened, EventType.WrongSpotOpened);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
    }

    private void HalfToBottle(CallbackData data) {
        Bottle bottle = data.DataObject as Bottle;
        LiquidContainer container = bottle.Container;
        if (container.LiquidType == LiquidType.Soycaseine) {
            filterHalvesInSoycaseine++;
        }

        if (container.LiquidType == LiquidType.Tioglygolate) {
            filterHalvesInTioglycolate++;
        }

        if (filterHalvesInSoycaseine + filterHalvesInTioglycolate >= 2) {
            EnableCondition(Conditions.HavlesInBottles);
            CheckMistakes();
            CompleteTask();
        }
    }

    private void TouchedFilter(CallbackData data) {
        CreateTaskMistake("Koskit kalvosuodattimeen kädellä", 1);
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
    }

    private void CheckIfInsideLaminarCabinet(Interactable interactable) {
        if (laminarCabinet.GetContainedItems().Contains(interactable)) {
            return;
        } else {
            CreateTaskMistake("Avasit suojapakkauksen laminaarikaapin ulkopuolella!!!", 1);
        }

        if (laminarCabinet.GetContainedItems() == null) {
            CreateTaskMistake("Avasit suojapakkauksen laminaarikaapin ulkopuolella!!!", 1);
        }
    }

    public void WrongSpotOpened(CallbackData data) {
        CreateTaskMistake("Avasit suojapakkauksen väärästä päästä!", 1);
    }
    private void CheckMistakes() {
        if (filterHalvesInSoycaseine > 1 || filterHalvesInTioglycolate > 1) {
            CreateTaskMistake("Kaksi puolikasta samassa liuoksessa", 1);
        }
    }
}