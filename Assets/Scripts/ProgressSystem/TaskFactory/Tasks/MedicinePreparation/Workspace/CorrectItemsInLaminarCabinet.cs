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
    private int smallSyringes = 0;
    private int objectCount = 0;
    private bool firstCheckDone = false;
    
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
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        base.SubscribeEvent(CorrectItems, EventType.CorrectItemsInLaminarCabinet);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }
    }

    /// <summary>
    /// Once fired by an event, checks which item was picked and sets the corresponding condition to be true.
    /// </summary>
    private void CorrectItems(CallbackData data) {
        if (laminarCabinet == null) {
            Popup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }
        List<GameObject> objects = laminarCabinet.GetContainedItems();
        if (objects.Count == 0) {
            Popup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }
        objectCount = objects.Count;

        CheckConditions(objects);

        CompleteTask();
        if (!IsCompleted()) {
            MissingItems();
        }
    } 
    #endregion

    #region Private Methods

    private void CheckConditions(List<GameObject> objects) {
        SyringeCapFactoryEnabled();
        foreach(GameObject value in objects) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            switch (type) {
                case ObjectType.Syringe:
                    Syringe syringe = item as Syringe;
                    if (syringe.Container.Capacity == 20000) {
                        EnableCondition(Conditions.BigSyringe); 
                    } else if (syringe.Container.Capacity == 1000) {
                        smallSyringes++;
                        if (smallSyringes == 6) {
                            EnableCondition(Conditions.SmallSyringes);
                        }
                    }
                    break;
                case ObjectType.Needle:
                    EnableCondition(Conditions.Needle); 
                    break;
                case ObjectType.Luerlock:
                    EnableCondition(Conditions.Luerlock);
                    break;
                case ObjectType.Bottle:
                    EnableCondition(Conditions.MedicineBottle);
                    break;
            }
        }   
    }

    private void SyringeCapFactoryEnabled() {
        if (laminarCabinet.CapFactoryEnabled) {
            EnableCondition(Conditions.SyringeCap);
        }
    }

    private void MissingItems() {
        if (!firstCheckDone) {
            Popup("Työvälineitä puuttuu.", MsgType.Mistake, -2);
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectItemsInLaminarCabinet, 2);
            firstCheckDone = true;
        } else {
            Popup("Työvälineitä puuttuu.", MsgType.Mistake);
        }
        smallSyringes = 0;
        DisableConditions();
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
        if (objectCount == 12) {
            Popup("Oikea määrä työvälineitä.", MsgType.Notify, 2);
        } else {
            Popup("Liikaa työvälineitä.", MsgType.Mistake, -1);
            G.Instance.Progress.Calculator.Subtract(TaskType.CorrectItemsInLaminarCabinet);
        }
    }
    #endregion
}