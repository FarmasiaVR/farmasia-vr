using System.Collections.Generic;
using UnityEngine;
public class LuerlockAttach : TaskBase {
    #region Fields
    private string[] conditions = { "LuerlockAttached", "RightPositionOfLuerlock", "PreviousTaskCompletion" };
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe};
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for LuerlockAttach task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public LuerlockAttach() : base(TaskType.LuerlockAttach, true, true) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(AttachLuerlock, EventType.AttachLuerlock);
    }
    /// <summary>
    /// Once fired by an event, checks if and how Luerlock was attached as well as previous required task completion.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void AttachLuerlock(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Luerlock) {
            EnableCondition("LuerlockAttached");
            if (CheckLuerlockPosition(item)) {
                EnableCondition("RightPositionOfLuerlock");
            }
        }

        if (CheckPreviousTaskCompletion(requiredTasks)) {
            EnableCondition("PreviousTaskCompletion");
        }

        bool check = CheckClearConditions(true);
        if (!check && base.clearConditions["LuerlockAttached"] && base.clearConditions["PreviousTaskCompletion"]) {
            UISystem.Instance.CreatePopup(-1, "Luerlock was not successfully attached", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.LuerlockAttach);
            base.FinishTask();
        }
    }
    /// <summary>
    /// Method checks whether the given item has been placed correctly.
    /// </summary>
    /// <param name="item">"Refers to an item object."</param>
    /// <returns>"Returns true if the item has been correctly attached to the Luerlock object."</returns>
    private bool CheckLuerlockPosition(GeneralItem item) {
        //missing code, check the position of attached Luerlock
        return false;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Luerlock was successfully attached", MessageType.Notify);
        G.Instance.Progress.Calculator.Add(TaskType.LuerlockAttach);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Luerlock-to-luerlock-välikappaleen kiinnitys.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Kiinnitä Luerlock-to-luerlock-välikappale oikein 20ml ruiskuun.";
    }
    #endregion
}