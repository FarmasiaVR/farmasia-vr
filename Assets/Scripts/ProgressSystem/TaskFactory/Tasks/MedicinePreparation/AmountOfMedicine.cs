using System.Collections.Generic;
using UnityEngine;
public class AmountOfMedicine : TaskBase {
    #region Fields
    private string[] conditions = { "RightAmountOfMedicine", "PreviousTasksCompleted" };
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe, TaskType.LuerlockAttach, TaskType.SyringeAttach};
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for AmountOfMedicine task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public AmountOfMedicine() : base(TaskType.AmountOfMedicine, true, true) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(Medicine, EventType.AmountOfMedicine);
    }
    /// <summary>
    /// Once fired by an event, checks if right amount has been chosen and if required previous tasks are completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void Medicine(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Medicine) {
            if (AmountRight(item)) {
                EnableCondition("RightAmountOfMedicine");
            }
        }  
        
        if (CheckPreviousTaskCompletion(requiredTasks)) {
            EnableCondition("PreviousTasksCompleted");
        }

        bool check = CheckClearConditions(true);
        if (!check && base.clearConditions["PreviousTasksCompleted"]) {
            UISystem.Instance.CreatePopup(-1, "Wrong amount of medicine was taken", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.AmountOfMedicine);
            base.FinishTask();
        }
    }
    /// <summary>
    /// Method checks if the size of a given item corresponds to the one expected.
    /// </summary>
    /// <param name="item">"Refers to an item with a size."</param>
    /// <returns>"Returns true if the condition is fulfilled."</returns>
    private bool AmountRight(GeneralItem item) {
        //code missing, checks if the size of medicine object is correct
        return false;
    }   
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Right amount of medicine", MessageType.Notify);
        G.Instance.Progress.Calculator.Add(TaskType.AmountOfMedicine);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Vedä ruiskuun lääkettä.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.";
    }
    #endregion
}