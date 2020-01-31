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
        base.SubscribeEvent(DisinfectBottleCap, EventType.BottleDisinfect);
    }

    /// <summary>
    /// Once fired by an event, checks if bottle cap was disinfected and previous tasks completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">.</param>
    private void DisinfectBottleCap(CallbackData data) {
        FinishTask();
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        base.FinishTask();
    }

    public override string GetDescription() {
        return base.GetDescription();
    }

    public override string GetHint() {
        return "";
    }

    protected override void OnTaskComplete() {
        //throw new NotImplementedException();
    }
    #endregion
}