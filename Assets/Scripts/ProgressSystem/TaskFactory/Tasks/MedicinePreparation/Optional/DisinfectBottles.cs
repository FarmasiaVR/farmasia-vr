using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Disinfect bottle cork. If not done, object will be contaminated.
/// </summary>
public class DisinfectBottles : TaskBase {

    #region Constructor
    ///  <summary>
    ///  Constructor for Disinfect task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public DisinfectBottles() : base(TaskType.DisinfectBottles, true, true) {
        Subscribe();
        points = 1;
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
        G.Instance.Progress.ForceCloseTask(taskType, false);
    }
    #endregion

    #region Public Methods

    public override string GetDescription() {
        return base.GetDescription();
    }

    public override string GetHint() {
        return "";
    }

    protected override void OnTaskComplete() {

        CabinetBase cabinet = null;
        foreach (CabinetBase c in GameObject.FindObjectsOfType<CabinetBase>()) {
            if (c.type == CabinetBase.CabinetType.Laminar) {
                cabinet = c;
            }
        }

        foreach (Interactable interactable in cabinet.GetContainedItems()) {
            GeneralItem g = interactable as GeneralItem;
            if (g != null && !g.IsClean) {
                Popup("Pullon korkkia ei puhdistettu", MsgType.Error);
                G.Instance.Progress.Calculator.SubtractWithScore(taskType, 1);
                return;
            }
        }
    }
    #endregion
}