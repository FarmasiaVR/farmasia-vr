using System;
using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
/// <summary>
/// Correct amount of items inserted into Fume Cupboard.
/// </summary>
public class CorrectItemsInLaminarCabinetMembrane: Task {

    public override string Description { get => "Siirrä valitsemasi työvälineet laminaarikaappiin"; }

    #region Fields
    public enum Conditions { Bottles100ml, PeptoniWaterBottle, SoycaseineBottle, TioglycolateBottle, Tweezers, Scalpel, Pipette, SoycaseinePlate, SabouradDextrosiPlate, Pump, PumpFilter, SterileBag, Pencil }
    private CabinetBase laminarCabinet;
    public int dirtyItems = 0;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectItemsInLaminarCabinetMembrane task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectItemsInLaminarCabinetMembrane() : base(TaskType.CorrectItemsInLaminarCabinetMembrane, false) {
        SetCheckAll(true);
        
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CheckLaminarCabinet, EventType.CheckLaminarCabinetItems);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase) data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    /// <summary>
    /// Once fired by an event, checks which item was picked and sets the corresponding condition to be true.
    /// </summary>
    private void CheckLaminarCabinet(CallbackData data) {
        if (laminarCabinet == null) {
            Logger.Error("laminarCabinet was null in CorrectItemsInLaminarCabinetMembrane!");
            return;
        }
        List<Interactable> objects = laminarCabinet.GetContainedItems();
        if (objects.Count == 0) {
            Popup(Translator.Translate("XR MembraneFilteration 2.0", "MoveItemsToLaminarCabinet"), MsgType.Notify);
            return;
        }

        CheckItems();
        CompleteTask();
    }
    #endregion

    #region Public Methods

    // public override string Hint { get => base.hint + laminarCabinet?.GetMissingItems(); }

    private void CheckItems() {
        Logger.Print("Checking cabinet items if they are correct");
        int bottles100ml = 0;
        int peptonWaterBottle = 0;
        int soycaseineBottle = 0;
        int tioglycolateBottle = 0;
        int soycaseinePlate = 0;
        int sabouradDextrosiPlate = 0;
        int tweezers = 0;
        int scalpel = 0;
        int pipette = 0;
        int sterileBag = 0;
        int pump = 0;
        int filter = 0;

        int uncleanCount = 0;

        foreach (var item in laminarCabinet.GetContainedItems()) {
            if (Interactable.GetInteractable(item.transform) is var g && g != null) {
                if (g is Bottle bottle) {
                    int capacity = bottle.Container.Capacity;
                    LiquidType type = bottle.Container.LiquidType;
                    if (capacity == 100000) {
                        bottles100ml++;
                        if (bottles100ml == 4) {
                            EnableCondition(Conditions.Bottles100ml);
                        }

                    } else if (type == LiquidType.Peptonwater) {
                        peptonWaterBottle++;
                        EnableCondition(Conditions.PeptoniWaterBottle);
                    } else if (type == LiquidType.Soycaseine) {
                        soycaseineBottle++;
                        EnableCondition(Conditions.SoycaseineBottle);
                    } else if (type == LiquidType.Tioglygolate) {
                        tioglycolateBottle++;
                        EnableCondition(Conditions.TioglycolateBottle);
                    }
                } else if (g is AgarPlateLid lid) {
                    string variant = lid.Variant;
                    if (variant == "Soija-kaseiini") {
                        soycaseinePlate++;
                        if (soycaseinePlate == 3)
                        {
                            EnableCondition(Conditions.SoycaseinePlate);
                        }
                    } else if (variant == "Sabourad-dekstrosi") {
                        sabouradDextrosiPlate++;
                        EnableCondition(Conditions.SabouradDextrosiPlate);
                    }
                } else if (g is Tweezers) {
                    EnableCondition(Conditions.Tweezers);
                    tweezers++;
                } else if (g is Scalpel) {
                    EnableCondition(Conditions.Scalpel);
                    scalpel++;
                } else if (g is Pipette || g is BigPipette || g is PipetteContainer || g is PipetteHeadCover) {
                    pipette++;
                    if (pipette >= 4) {
                        EnableCondition(Conditions.Pipette);
                    }
                } else if (g is Pump) {
                    EnableCondition(Conditions.Pump);
                    pump++;
                } else if (g is FilterInCover) {
                    EnableCondition(Conditions.PumpFilter);
                    filter++;
                
                } else if (g is SterileBag2) {
                    EnableCondition(Conditions.SterileBag);
                    sterileBag++;
                } 
                else if(g is WritingPen)
                {
                    EnableCondition(Conditions.Pencil);
                }
                if (g is GeneralItem generalItem && !generalItem.IsClean && !(generalItem is Bottle)) {
                    Debug.Log("item was contaminated: " + g.gameObject.name);
                    uncleanCount++;
                    Logger.Warning(g.name + " in laminar cabinet was filthy.");
                }
            }
        }

        if (bottles100ml == 4 && peptonWaterBottle == 1 && soycaseineBottle == 1 && tioglycolateBottle == 1 && soycaseinePlate == 3 && sabouradDextrosiPlate == 1 && tweezers == 1 && scalpel == 1 && pipette == 3 && pump == 1 && filter == 1 && sterileBag == 1) {
            Logger.Print("All done");
            
        }

        if (uncleanCount != dirtyItems) {
            if (uncleanCount > dirtyItems)
            {
                CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "DirtyItemInLaminarCabinet"), uncleanCount);
                dirtyItems++;
            }
            else 
            {
                dirtyItems--;
            }
        }
    }
    #endregion
}
