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
    public enum Conditions { BigSyringe, SmallSyringes, Needle, Luerlock, MedicineBottle }
    private int smallSyringes;
    private int objectCount;
    private int checkTimes;
    
    private CabinetBase laminarCabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectItemsInLaminarCabinet task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectItemsInLaminarCabinet() : base(TaskType.CorrectItemsInLaminarCabinet, true, false) {
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        SetItemsToZero();
        checkTimes = 0;
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
            UISystem.Instance.CreatePopup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }
        List<GameObject> objects = laminarCabinet.GetContainedItems();
        if (objects.Count == 0) {
            UISystem.Instance.CreatePopup("Siirrä tarvittavat työvälineet laminaarikaappiin.", MsgType.Notify);
            return;
        }
        checkTimes++;
        objectCount = objects.Count;

        CheckConditions(objects);
        
        bool check = CheckClearConditions(true);
        if (!check) {
            MissingItems(checkTimes);
        }
    } 
    #endregion

    #region Private Methods
    private void SetItemsToZero() {
        smallSyringes = 0;
    }

    private void CheckConditions(List<GameObject> objects) {
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

    private void MissingItems(int checkTimes) {
        if (checkTimes == 1) {
            UISystem.Instance.CreatePopup(0, "Työvälineitä puuttuu.", MsgType.Mistake);
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectItemsInLaminarCabinet, 2);
        } else {
            UISystem.Instance.CreatePopup("Työvälineitä puuttuu.", MsgType.Mistake);
        }
        SetItemsToZero();
        DisableConditions();
    }
    #endregion 

    #region Public Methods
    public override void FinishTask() {
        if (checkTimes == 1) {
            // 1 disinfect cloth + 6 small syringes + 1 big syringe + 1 luerlock + 1 needle + 1 bottle = 11 items
            if (objectCount == 11) {
                UISystem.Instance.CreatePopup(2, "Oikea määrä työvälineitä.", MsgType.Notify);
            } else {
                UISystem.Instance.CreatePopup(1, "Liikaa työvälineitä.", MsgType.Notify);
                G.Instance.Progress.Calculator.Subtract(TaskType.CorrectItemsInLaminarCabinet);
            }
        }
        base.FinishTask();
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        string missingItemsHint = laminarCabinet.GetMissingItems();
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Tarkista valintasi painamalla laminaarikaapin tarkistusnappia. " + missingItemsHint; 
    }
    #endregion
}