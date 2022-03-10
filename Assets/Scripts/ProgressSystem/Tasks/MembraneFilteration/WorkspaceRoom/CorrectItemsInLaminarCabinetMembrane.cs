using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of items inserted into Fume Cupboard.
/// </summary>
public class CorrectItemsInLaminarCabinetMembrane : TaskBase
{

    #region Constants
    private const string DESCRIPTION = "Siirrä valitsemasi työvälineet laminaarikaappiin ja paina kaapin tarkistusnappia.";
    #endregion

    #region Fields
    public enum Conditions { Bottles100ml, PeptoniWaterBottle, SoycaseineBottle, TioglycolateBottle, Tweezers, Scalpel, Pipette, SoycaseinePlate, SabouradDextrosiPlate, Pump, PumpFilter, SterileBag, CleaningBottle}
    private int objectCount;
    private int correctItemCount = 19;
    private CabinetBase laminarCabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectItemsInLaminarCabinetMembrane task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectItemsInLaminarCabinetMembrane() : base(TaskType.CorrectItemsInLaminarCabinetMembrane, true, false)
    {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe()
    {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CheckLaminarCabientButton, EventType.CheckLaminarCabinetItems);
    }

    private void SetCabinetReference(CallbackData data)
    {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar)
        {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    /// <summary>
    /// Once fired by an event, checks which item was picked and sets the corresponding condition to be true.
    /// </summary>
    private void CheckLaminarCabientButton(CallbackData data)
    {
        if (laminarCabinet == null)
        {
            Popup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }
        List<Interactable> objects = laminarCabinet.GetContainedItems();
        if (objects.Count == 0)
        {
            Popup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }

        CheckItems();
        CompleteTask();

        //G.Instance.Progress.ForceCloseTask(taskType, false);
    }
    #endregion

    #region Public Methods

    public override string GetDescription()
    {
        return DESCRIPTION;
    }

    public override string GetHint()
    {
        string missingItemsHint = laminarCabinet.GetMissingItems();
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Tarkista valintasi painamalla laminaarikaapin tarkistusnappia. " + missingItemsHint;
    }

    protected override void OnTaskComplete() { /* Nothing */ }

    private void CheckItems()
    {
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
        int cleaningBottle = 0;


        int uncleanCount = 0;

        foreach (var item in laminarCabinet.GetContainedItems())
        {
            if (Interactable.GetInteractable(item.transform) is var g && g != null)
            {
                if (g is Bottle bottle)
                {
                    int capacity = bottle.Container.Capacity;
                    LiquidType type = bottle.Container.LiquidType;
                    if (capacity == 100000)
                    {
                        bottles100ml++;
                        if (bottles100ml == 6)
                        {
                            EnableCondition(Conditions.Bottles100ml);
                        }

                    }
                    else if (type == LiquidType.Peptonwater)
                    {
                        peptonWaterBottle++;
                        EnableCondition(Conditions.Bottles100ml);
                    }
                    else if (type == LiquidType.Soycaseine)
                    {
                        soycaseineBottle++;
                        EnableCondition(Conditions.Bottles100ml);
                    }
                    else if (type == LiquidType.Tioglygolate)
                    {
                        tioglycolateBottle++;
                        EnableCondition(Conditions.Bottles100ml);
                    }

                    else
                    {
                        CreateTaskMistake("Väärä pullo laminaarikaapissa", 5);
                    }
                }

                else if (g is AgarPlateLid lid)
                {
                    string variant = lid.Variant;
                    if (variant == "Soija-kaseiini")
                    {
                        soycaseinePlate++;
                        EnableCondition(Conditions.SoycaseinePlate);
                    }
                    else if (variant == "Sabourad-dekstrosi")
                    {
                        sabouradDextrosiPlate++;
                        EnableCondition(Conditions.SoycaseinePlate);
                    }
                    else
                    {
                        CreateTaskMistake("Väärä agarmalja laminaarikaapissa", 5);
                    }
                        
                }
                else if (g is Tweezers)
                {
                    EnableCondition(Conditions.Tweezers);
                    tweezers++;
                }
                else if (g is Scalpel)
                {
                    EnableCondition(Conditions.Scalpel);
                    scalpel++;
                }
                else if (g is Pipette || g is BigPipette)
                {
                    EnableCondition(Conditions.Pipette);
                    pipette++;
                }
                else if (g is Pump)
                {
                    EnableCondition(Conditions.Pump);
                    pump++;
                }
                else if (g is PumpFilter)
                {
                    EnableCondition(Conditions.PumpFilter);
                    filter++;
                }
                else if (g is SterileBag)
                {
                    EnableCondition(Conditions.SterileBag);
                    sterileBag++;
                }
                else if (g is CleaningBottle)
                {
                    EnableCondition(Conditions.CleaningBottle);
                    cleaningBottle++;
                }
                if (g is GeneralItem generalItem && !generalItem.IsClean)
                {
                    uncleanCount++;
                    Logger.Warning(g.name + " in laminar cabinet was not clean");
                }
            }
        }

        if (bottles100ml == 4 && peptonWaterBottle == 1 && soycaseineBottle == 1 && tioglycolateBottle == 1 && soycaseinePlate == 3 && sabouradDextrosiPlate == 1 && tweezers == 1 && scalpel == 1 && pipette == 1 && pump == 1 && filter == 1 && sterileBag == 1 && cleaningBottle == 1)
        {
            Logger.Print("All done");
            Popup("Oikea määrä työvälineitä laminaarikaapissa.", MsgType.Done, 2);
        }
        else
        {
            CreateTaskMistake("Väärä määrä työvälineitä laminaarikaapissa.", 2);

        }

        if (uncleanCount > 0)
        {
            CreateTaskMistake("Likainen esine laminaarikaapissa", uncleanCount);
        }
    }
    #endregion
}
