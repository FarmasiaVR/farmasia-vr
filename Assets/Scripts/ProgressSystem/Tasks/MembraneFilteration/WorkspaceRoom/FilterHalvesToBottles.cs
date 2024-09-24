using System;
using System.Diagnostics;
using FarmasiaVR.Legacy;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


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
            // UnityEngine.Debug.Log("soycaseine halves before: " + filterHalvesInSoycaseine);
            filterHalvesInSoycaseine++;
            // UnityEngine.Debug.Log("soycaseine halves after: " + filterHalvesInSoycaseine);


        }

        if (container.LiquidType == LiquidType.Tioglygolate) {
            // UnityEngine.Debug.Log("Tioglygolate halves before: " + filterHalvesInTioglycolate);

            filterHalvesInTioglycolate++;
            // UnityEngine.Debug.Log("Tioglygolate halves after: " + filterHalvesInTioglycolate);

        }

        if (filterHalvesInSoycaseine + filterHalvesInTioglycolate >= 2) {
            // UnityEngine.Debug.Log("starting task finish");
            EnableCondition(Conditions.HavlesInBottles);
            // UnityEngine.Debug.Log("starting check mistake");
            CheckMistakes();
            // UnityEngine.Debug.Log("starting complete task");
            CompleteTask();
            // UnityEngine.Debug.Log("verythings completed! WHOOOO");
        }
    }

    private void TouchedFilter(CallbackData data) {
        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "TouchedFilterHalves"), 1);
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
        // UnityEngine.Debug.Log("tweezers enabled");
        EnableCondition(Conditions.OpenedTweezersCover);
    }

    private void CheckIfInsideLaminarCabinet(Interactable interactable) {
        if (laminarCabinet.GetContainedItems().Contains(interactable)) {
            return;
        } else {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningProtectivePackageFilterHalves"), 1);
        }

        if (laminarCabinet.GetContainedItems() == null) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningProtectivePackageFilterHalves"), 1);
        }
    }

    public void WrongSpotOpened(CallbackData data) {
        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "OpeningWrongEndFilterHalves"), 1);
    }
    private void CheckMistakes() {
        if (filterHalvesInSoycaseine > 1 || filterHalvesInTioglycolate > 1) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "BothFilterHalvesInSame"), 1);
        }
    }
}