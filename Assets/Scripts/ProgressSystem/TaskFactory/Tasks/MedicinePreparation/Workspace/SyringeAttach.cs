using System.Collections.Generic;
using UnityEngine;
public class SyringeAttach : TaskBase {
    #region Fields
    private string[] conditions = { "SyringeAttached", "RightSyringeSize", "PreviousTaskCompletion" };
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.LuerlockAttach};
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for SyringeAttach task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public SyringeAttach() : base(TaskType.SyringeAttach, true, true) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(AttachSyringe, EventType.AttachSyringe);
    }
    /// <summary>
    /// Once fired by an event, checks if syringe was attached to Luerlock, which syringe size was chosen
    /// as well as previous required task completion.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void AttachSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            //check that syringe was attached to luerlock
            EnableCondition("SyringeAttached");
            if (SyringeSize(item)) {
                EnableCondition("RightSyringeSize");
            }
        }

        if (CheckPreviousTaskCompletion(requiredTasks)) {
            EnableCondition("PreviousTaskCompletion");
        }

        bool check = CheckClearConditions(true);
        if (!check && base.clearConditions["SyringeAttached"] && base.clearConditions["PreviousTaskCompletion"]) {
            UISystem.Instance.CreatePopup(-1, "Wrong syringe size was chosen", MessageType.Mistake);
            G.Instance.Progress.calculator.Subtract(TaskType.SyringeAttach);
            base.FinishTask();
        }
    }
    /// <summary>
    /// Method checks the size of the given item.
    /// </summary>
    /// <param name="item">"Refers to an item with a size."</param>
    /// <returns>"Returns true if the item has the expected size."</returns>
    private bool SyringeSize(GeneralItem item) {
        Syringe syringe = item as Syringe;
            if (syringe.Container.Capacity == 1) {
                return true;
            }
        return false;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Right syringe size was chosen", MessageType.Notify);
        G.Instance.Progress.calculator.Add(TaskType.SyringeAttach);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Yhdistä Luerlock-to-luerlock-välikappaleeseen toinen ruisku.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Kiinnitä Luerlock-to-luerlock-välikappaleeseen myös 1.0ml ruisku.";
    }
    #endregion
}