using System;
using System.Collections.Generic;
using UnityEngine;
public class SyringeAttach : TaskBase {
    #region Fields
    public enum Conditions { SmallSyringesAttached }
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe, TaskType.LuerlockAttach};
    private int syringes;
    private int smallSyringes;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for SyringeAttach task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public SyringeAttach() : base(TaskType.SyringeAttach, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        syringes = 0;
        smallSyringes = 0;
        points = 1;
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
        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            syringes++;
            Syringe syringe = item.GetComponent<Syringe>();
            if (syringe.Container.Capacity == 1000) {
                smallSyringes++;
            }
        }

        if (smallSyringes == 6) {
            EnableCondition(Conditions.SmallSyringesAttached);
        }
        if (syringes == 6) {
            bool check = CheckClearConditions(true);
            if (!check) {
                UISystem.Instance.CreatePopup(0, "Wrong syringe size was chosen for one of the syringes", MessageType.Mistake);
                G.Instance.Progress.Calculator.Subtract(TaskType.SyringeAttach);
                base.FinishTask();
            }
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Right syringe sizes were chosen", MessageType.Notify);
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