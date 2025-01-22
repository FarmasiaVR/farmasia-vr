using System;
using System.Collections.Generic;
using System.Diagnostics;
using FarmasiaVR.Legacy;
using Unity;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

class FillBottles : Task {
    public enum Conditions { BottlesFilled }

    private int soycaseineBottlesDone = 0;
    private int tioglygolateBottlesDone = 0;

    private readonly int REQUIRED_AMOUNT = 80000;

    private HashSet<Bottle> bottles = new HashSet<Bottle>();

    public FillBottles() : base(TaskType.FillBottles, false) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        SubscribeEvent(OnBottleFill, EventType.TransferLiquidToBottle);
    }

    private void OnBottleFill(CallbackData data) {
        LiquidContainer container = data.DataObject as LiquidContainer;
        soycaseineBottlesDone = 0;
        tioglygolateBottlesDone = 0;
        if (container.GeneralItem is Bottle bottle && bottle.ObjectType == ObjectType.Bottle) {
            if (bottle.Container.Amount >= REQUIRED_AMOUNT) {
                //if (bottles.Contains(bottle)) return;
                if (!bottles.Contains(bottle))
                {
                    bottles.Add(bottle);
                }
                UnityEngine.Debug.Log("current tio amount in bottle: " + bottle.Container.Amount);
                    
                foreach (Bottle b in bottles)
                {
                    if (b.Container.LiquidType == LiquidType.Soycaseine && b.Container.Amount >= REQUIRED_AMOUNT)
                    {
                        soycaseineBottlesDone++;
                    }
                    if (b.Container.LiquidType == LiquidType.Tioglygolate && b.Container.Amount >= REQUIRED_AMOUNT)
                    {
                        tioglygolateBottlesDone++;
                    }
                }
                //if (bottle.Container.LiquidType == LiquidType.Soycaseine) {
                  //  soycaseineBottlesDone++;
                //} else if (bottle.Container.LiquidType == LiquidType.Tioglygolate) {
                  //  tioglygolateBottlesDone++;
                //}
            }
        }
        UnityEngine.Debug.Log("tio and soy amount after monster: tio:" + tioglygolateBottlesDone + "    soy:" + soycaseineBottlesDone);
        if (soycaseineBottlesDone >= 2 && tioglygolateBottlesDone >= 2) {
            EnableCondition(Conditions.BottlesFilled);
            CheckMistakes();
            CompleteTask();
        }
    }

    private void CheckMistakes() {
        foreach (var bottle in bottles) {

            var writable = bottle.GetComponent<Writable>();
            if (bottle.Container.LiquidType == LiquidType.Soycaseine) {

                if (!writable.WrittenLines.ContainsKey(WritingType.SoyCaseine)) {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongWritingSoyCaseineBottle"), 1);
                }
                if (bottle.Container.Amount > REQUIRED_AMOUNT) {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongAmountSoyCaseineBottle"), 1);
                }
                if (bottle.Container.Impure) {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "MixedLiquidSoyCaseineBottle"), 1);
                }

            } else if (bottle.Container.LiquidType == LiquidType.Tioglygolate) {

                if (!writable.WrittenLines.ContainsKey(WritingType.Tioglygolate)) {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongWritingTioglygolateBottle"), 1);
                }
                if (bottle.Container.Amount > REQUIRED_AMOUNT) {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongAmountTioglygolateBottle"), 1);
                }
                if (bottle.Container.Impure) {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "MixedLiquidTioglygolateBottle"), 1);
                }
            }

            
        }
    }
}
