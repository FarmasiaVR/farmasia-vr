using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class FilterHalvesToBottles : Task {
    public enum Conditions { HavlesInBottles }

    private int filterHalvesInSoycaseine = 0;
    private int filterHalvesInTioglycolate = 0;

    public FilterHalvesToBottles() : base(TaskType.FilterHalvesToBottles, true) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        SubscribeEvent(HalfToBottle, EventType.FilterHalfEnteredBottle);
        SubscribeEvent(TouchedFilter, EventType.TouchedFilterWithHand);
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
        CreateTaskMistake("Koskit filteriin kädellä", 1);
    }
    private void CheckMistakes() {
        if (filterHalvesInSoycaseine > 1 || filterHalvesInTioglycolate > 1) {
            CreateTaskMistake("Kaksi puolikasta samassa liuoksessa", 1);
        }
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}