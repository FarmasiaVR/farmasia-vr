using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of items inserted into Fume Cupboard.
/// </summary>
public class CorrectItemsInLaminarCabinet : TaskBase {

    #region Constants
    private const string DESCRIPTION = "Siirrä valitsemasi työvälineet laminaarikaappiin ja paina kaapin tarkistusnappia.";
    #endregion

    #region Fields
    public enum Conditions { BigSyringe, SmallSyringes, Needle, Luerlock, MedicineBottle, SyringeCap }
    private CabinetBase laminarCabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectItemsInLaminarCabinet task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectItemsInLaminarCabinet() : base(TaskType.CorrectItemsInLaminarCabinet, true, false) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 2;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CheckLaminarCabientButton, EventType.CheckLaminarCabinetItems);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    /// <summary>
    /// Once fired by an event, checks which item was picked and sets the corresponding condition to be true.
    /// </summary>
    private void CheckLaminarCabientButton(CallbackData data) {
        if (laminarCabinet == null) {
            Popup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }
        List<Interactable> objects = laminarCabinet.GetContainedItems();
        if (objects.Count == 0) {
            Popup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }

        CheckItems();
        CompleteTask();

        //G.Instance.Progress.ForceCloseTask(taskType, false);
    } 
    #endregion

    #region Public Methods

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        string missingItemsHint = laminarCabinet.GetMissingItems();
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Tarkista valintasi painamalla laminaarikaapin tarkistusnappia. " + missingItemsHint; 
    }

    protected override void OnTaskComplete() { /* Nothing */ }

    private void CheckItems() {
        Logger.Print("Checking cabinet items if they are correct");
        int syringeCount = 0;

        int luerlockCount = 0;
        int needleCount = 0;
        int bottleCount = 0;
        bool correctBottle = false;

        int uncleanCount = 0;

        foreach (var item in laminarCabinet.GetContainedItems()) {
            if (Interactable.GetInteractable(item.transform) is var g && g != null) {
                if (g is SmallSyringe) {
                    syringeCount++;
                    if (syringeCount == 6) {
                        EnableCondition(Conditions.SmallSyringes);
                    }
                } else if (g is Syringe) {
                    EnableCondition(Conditions.BigSyringe);
                } else if (g is LuerlockAdapter) {
                    EnableCondition(Conditions.Luerlock);
                    luerlockCount++;
                } else if (g is Needle) {
                    EnableCondition(Conditions.Needle);
                    needleCount++;
                } else if (g is MedicineBottle bottle) {
                    bottleCount++;

                    int capacity = bottle.Container.Capacity;
                    if (capacity == 4000) {
                        EnableCondition(Conditions.MedicineBottle);
                        correctBottle = true;
                    } else if (capacity == 100000) {
                        CreateTaskMistake("Väärä pullo laminaarikaapissa", 5);
                    }
                } else if (g is SyringeCapBag) {
                    EnableCondition(Conditions.SyringeCap);
                }

                if (g is GeneralItem generalItem && !generalItem.IsClean && !(generalItem is MedicineBottle)) {
                    uncleanCount++;
                    Logger.Warning(g.name + " in laminar cabinet was not clean");
                }
            }
        }

        if (syringeCount == 6 && luerlockCount == 1 && bottleCount == 1 && correctBottle && needleCount == 1) {
            Logger.Print("All done");
            Popup("Oikea määrä työvälineitä laminaarikaapissa.", MsgType.Done, 2);
        } else {
            CreateTaskMistake("Väärä määrä työvälineitä laminaarikaapissa.", 2);

        }

        if (uncleanCount > 0) {
            CreateTaskMistake("Likainen esine laminaarikaapissa", uncleanCount);
        }
    }
    #endregion
}