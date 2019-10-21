using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of medicine pulled into smaller syringes through LuerLock.
/// </summary>
public class CorrectAmountOfMedicineSelected : TaskBase {
    #region Fields
    private string[] conditions = { "SixSyringes", "RightAmountOfMedicine", "PreviousTasksCompleted" };
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe, TaskType.LuerlockAttach, TaskType.SyringeAttach};
    private int syringes;
    private int rightAmountInSyringes;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectAmountOfMedicineSelected task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public CorrectAmountOfMedicineSelected() : base(TaskType.CorrectAmountOfMedicineSelected, true, true) {
        Subscribe();
        AddConditions(conditions);
        syringes = 0;
        rightAmountInSyringes = 0;
        points = 6;
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
        if (G.Instance.Progress.currentPackage.name != "Workspace") {
            G.Instance.Progress.calculator.SubtractBeforeTime(TaskType.CorrectAmountOfMedicineSelected);
            UISystem.Instance.CreatePopup(-1, "Task tried before time", MessageType.Mistake);
            return;
        }
        if (CheckPreviousTaskCompletion(requiredTasks)) {
            EnableCondition("PreviousTasksCompleted");
        } else {
            return;
        }
        //check that happens in laminar cabinet
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            Syringe syringe = item.GetComponent<Syringe>();
            //should be 0,15ml
            if (syringe.Container.Capacity == 1) {
                syringes++;
                if (syringe.Container.Amount == 15) {
                    rightAmountInSyringes++;
                } 
            } 
        }  
        if (syringes == 6) {
            EnableCondition("SixSyringes");
        }
        if (rightAmountInSyringes == 6) {
            EnableCondition("RightAmountOfMedicine");
        }

        bool check = CheckClearConditions(true);
        if (!check && base.clearConditions["SixSyringes"]) {
            UISystem.Instance.CreatePopup(-1, "Wrong amount of medicine was taken", MessageType.Mistake);
            G.Instance.Progress.calculator.Subtract(TaskType.CorrectAmountOfMedicineSelected);
            base.FinishTask();
        }
    }  
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Right amount of medicine", MessageType.Notify);
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