using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of items inserted into Fume Cupboard.
/// </summary>
public class CorrectItemsInLaminarCabinet : TaskBase {
    #region Fields
    public enum Conditions { BigSyringe, SmallSyringes, /*Needles, */Luerlock, RightSizeBottle }
    private int smallSyringes, needles;
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
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        base.SubscribeEvent(CorrectItems, EventType.CorrectItemsInLaminarCabinet);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
        }
        base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
    }

    /// <summary>
    /// Once fired by an event, checks which item was picked and sets the corresponding condition to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void CorrectItems(CallbackData data) {
        if (!data.DataBoolean) {
            UISystem.Instance.CreatePopup("Turn on the laminar cabinet ventilation", MessageType.Notify);
            return;
        }

        if (laminarCabinet == null) {
            return;
        }
        List<GameObject> objects = laminarCabinet.GetContainedItems();
        if (objects.Count == 0) {
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
        needles = 0;
    }

    private void CheckConditions(List<GameObject> objects) {
        foreach(GameObject value in objects) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            switch (type) {
                case ObjectType.Syringe:
                    Syringe syringe = item as Syringe;
                    if (syringe.Container.Capacity == 5000) {
                        EnableCondition(Conditions.BigSyringe); 
                    } else if (syringe.Container.Capacity == 1000) {
                        smallSyringes++;
                        if (smallSyringes == 6) {
                            EnableCondition(Conditions.SmallSyringes);
                        }
                    }
                    break;
                /*case ObjectType.Needle:
                    needles++;
                    if (needles == 7) {
                        EnableCondition(Conditions.Needles); 
                    }
                    break;*/
                case ObjectType.Luerlock:
                    EnableCondition(Conditions.Luerlock);
                    break;
                case ObjectType.Bottle:
                    MedicineBottle bottle = item as MedicineBottle;
                    if (bottle.Container.Capacity == 80000) {
                        EnableCondition(Conditions.RightSizeBottle);
                    }
                    break;
            }
        }   
    }

    private void MissingItems(int checkTimes) {
        if (checkTimes == 1) {
            UISystem.Instance.CreatePopup(0, "Missing items", MessageType.Mistake);
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectItemsInLaminarCabinet, 2);
        } else {
            UISystem.Instance.CreatePopup("Missing items", MessageType.Mistake);
        }
        SetItemsToZero();
        DisableConditions();
    }
    #endregion 

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        if (checkTimes == 1) {
            // count changed from 16 to 9, needles missing
            if (objectCount == 9) {
                UISystem.Instance.CreatePopup(2, "Right amount of items", MessageType.Notify);
            } else {
                UISystem.Instance.CreatePopup(1, "Too many items", MessageType.Notify);
                G.Instance.Progress.Calculator.Subtract(TaskType.CorrectItemsInLaminarCabinet);
            }
        }
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Vie valitsemasi työvälineet laminaarikaappiin ja paina kaapin ilmanvaihto päälle.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        string missingItemsHint = laminarCabinet.GetMissingItems();
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Ilmanvaihto laitetaan päälle laminaarikaapin nappia painamalla. " + missingItemsHint; 
    }
    #endregion
}