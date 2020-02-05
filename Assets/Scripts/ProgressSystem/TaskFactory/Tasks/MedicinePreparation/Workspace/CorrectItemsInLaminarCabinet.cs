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

        G.Instance.Progress.ForceCloseTask(taskType, false);
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

    protected override void OnTaskComplete() {

        int syringeCount = 0;

        int luerlockCount = 0;
        int needleCount = 0;
        int bottleCount = 0;
        bool correctBottle = false;

        int uncleanCount = 0;

        foreach (var item in laminarCabinet.GetContainedItems()) {
            if (Interactable.GetInteractable(item.transform) as GeneralItem is var g && g != null) {
                if (g.ObjectType == ObjectType.Syringe) {
                    syringeCount++;
                } else if (g.ObjectType == ObjectType.Luerlock) {
                    luerlockCount++;
                } else if (g.ObjectType == ObjectType.Needle) {
                    needleCount++;
                } else if (g.ObjectType == ObjectType.Bottle) {
                    bottleCount++;

                    int capacity = ((MedicineBottle)g).Container.Capacity;
                    if (capacity == 4000) {
                        correctBottle = true;
                    }
                }

                if (!g.IsClean && g.ObjectType != ObjectType.Bottle) {
                    uncleanCount++;
                }
            }
        }

        if (syringeCount == 7 && luerlockCount == 1 && bottleCount == 1 && correctBottle && needleCount == 1) {
            Popup("Oikea määrä työvälineitä laminaarikaapissa.", MsgType.Done, 2);
        } else {
            Popup("Väärä määrä työvälineitä laminaarikaapissa.", MsgType.Mistake, -2);
            G.Instance.Progress.Calculator.SubtractWithScore(taskType, 2);
        }

        if (uncleanCount > 0) {
            G.Instance.Progress.Calculator.SubtractWithScore(taskType, uncleanCount);
            Popup("Likainen esine laminaarikaapissa", MsgType.Mistake, -uncleanCount);
        }
    }
    #endregion
}