using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Disinfect bottle cork. If not done, object will be contaminated.
/// </summary>
public class DisinfectBottles : TaskBase {
    #region Fields
    private bool allUsedBottlesWereDisinfected;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for Disinfect task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public DisinfectBottles() : base(TaskType.DisinfectBottles, true, true) {
        Subscribe();
        points = 1;
        allUsedBottlesWereDisinfected = true;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(DisinfectBottleCap, EventType.Disinfect);
    }

    /// <summary>
    /// Once fired by an event, checks if bottle cap was disinfected and previous tasks completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">.</param>
    private void DisinfectBottleCap(CallbackData data) {
        GeneralItem item = data.DataObject as GeneralItem;
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Bottle) {
            MedicineBottle bottle = item as MedicineBottle;
            if (!bottle.IsClean) {
                allUsedBottlesWereDisinfected = false;
            }
        }
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (!allUsedBottlesWereDisinfected) {
            G.Instance.Progress.Calculator.Subtract(TaskType.DisinfectBottles);
        }
        base.FinishTask();
        
    }

    public override string GetDescription() {
        return base.GetDescription();
    }

    public override string GetHint() {
        return "";
    }

    protected override void OnTaskComplete() {
        throw new NotImplementedException();
    }
    #endregion
}