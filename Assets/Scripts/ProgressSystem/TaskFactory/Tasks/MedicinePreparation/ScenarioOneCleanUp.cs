using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// In case syringes were dropped, this task is created to check if the player puts the dropped syringes to trash before finishing the game.
/// </summary>
public class ScenarioOneCleanUp : TaskBase {
    #region Fields
    private string[] conditions = { "SyringesPutToTrash", "PreviousTasksCompleted" };
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.CorrectAmountOfMedicineSelected};
    private int syringes;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ScenarioOneCleanUp task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public ScenarioOneCleanUp() : base(TaskType.ScenarioOneCleanUp, true, true) {
        Subscribe();
        AddConditions(conditions);
        syringes = 6;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(CleanUp, EventType.CleanUp);
    }
    /// <summary>
    /// Once fired by an event, checks if dropped syringes are put to trash and if required previous tasks are completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void CleanUp(CallbackData data) {
        if (CheckPreviousTaskCompletion(requiredTasks)) {
            EnableCondition("PreviousTasksCompleted");
        }
        
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            if (!base.clearConditions["PreviousTasksCompleted"]) {
                UISystem.Instance.CreatePopup(0, "Syringes were cleaned too early", MessageType.Mistake);
                G.Instance.Progress.calculator.Subtract(TaskType.ScenarioOneCleanUp);
                base.FinishTask(); 
                return;
            }
            syringes--;
            if (syringes == 0) {
                EnableCondition("SyringesPutToTrash");
            }
        }  
        
        bool check = CheckClearConditions(true);
        if (!check && base.clearConditions["PreviousTasksCompleted"]) {
            UISystem.Instance.CreatePopup(-1, "Syringes were not taken to trash", MessageType.Mistake);
            G.Instance.Progress.calculator.Subtract(TaskType.ScenarioOneCleanUp);
            base.FinishTask();
        }
    }   
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Syringes were taken to trash", MessageType.Notify);
        G.Instance.Progress.calculator.Add(TaskType.ScenarioOneCleanUp);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Siivoa lopuksi työtila.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Vie pelin aikana lattialle pudonneet lääkeruiskut roskakoriin.";
    }
    #endregion
}