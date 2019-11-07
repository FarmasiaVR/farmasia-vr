using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Disinfect bottle cork. If not done, object will be contaminated.
/// </summary>
public class DisinfectBottles : TaskBase {
    #region Fields
    public enum Conditions { BottleCapDisinfected }
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for Disinfect task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public DisinfectBottles() : base(TaskType.DisinfectBottles, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 1;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(DisinfectBottleCap, EventType.Disinfect);
    }
    /// <summary>
    /// Once fired by an event, checks if bottle cap was disinfected and previous tasks completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">.</param>
    private void DisinfectBottleCap(CallbackData data) {
        if (G.Instance.Progress.CurrentPackage.doneTypes.Contains(TaskType.CorrectItemsInLaminarCabinet)) {
            base.EnableCondition(Conditions.BottleCapDisinfected);
            UISystem.Instance.CreatePopup(1, "Pullon korkki putsattiin onnistuneesti.", MsgType.Done);
        } else {
            UISystem.Instance.CreatePopup(-1, "Korkkia ei putsattu.", MsgType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.DisinfectBottles);
        }
        base.UnsubscribeAllEvents();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        bool clear = base.CheckClearConditions(true);
        if (!clear) {
            G.Instance.Progress.Calculator.Subtract(TaskType.DisinfectBottles);
        }
        base.FinishTask();
    }

    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return base.GetDescription();
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "";
    }
    #endregion
}