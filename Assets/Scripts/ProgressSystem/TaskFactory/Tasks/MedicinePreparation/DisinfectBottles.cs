using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Disinfect bottle cork. If not done, object will be contaminated.
/// </summary>
public class DisinfectBottles : TaskBase {
    #region Fields
    private string[] conditions = { "BottleCapDisinfected", "PreviousTasksCompleted" };
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.CorrectItemsInLaminarCabinet, TaskType.CorrectLayoutInLaminarCabinet};
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for Disinfect task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public DisinfectBottles() : base(TaskType.DisinfectBottles, true, true) {
        Subscribe();
        AddConditions(conditions);
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
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void DisinfectBottleCap(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.BottleCap) {
            EnableCondition("BottleCapDisinfected");
        }

        if (CheckPreviousTaskCompletion(requiredTasks)) {
            EnableCondition("PreviousTasksCompleted");
        }
        
        bool check = CheckClearConditions(true);
        if (!check && base.clearConditions["PreviousTasksCompleted"]) {
            UISystem.Instance.CreatePopup(-1, "Bottle cap was not disinfected", MessageType.Mistake);
            G.Instance.Progress.calculator.Subtract(TaskType.DisinfectBottles);
            base.FinishTask();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Bottle cap disinfected", MessageType.Notify);
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
        return "Muista desinfioida lääkepullon korkki ennen seuraavaa työvaihetta.";
    }
    #endregion
}