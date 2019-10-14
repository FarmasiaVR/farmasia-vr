using System.Collections.Generic;
using UnityEngine;
public class MedicineToSyringe : TaskBase {
    #region Fields
    private string[] conditions = { "RightAmountInSyringe" };
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for MedicineToSyringe task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public MedicineToSyringe() : base(TaskType.MedicineToSyringe, true, true) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(ToSyringe, EventType.MedicineToSyringe);
    }
    /// <summary>
    /// Once fired by an event, checks which step of the MedicineToSyringe process has been taken and if required previous tasks are completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void ToSyringe(CallbackData data) {
        if (G.Instance.Progress.currentPackage.name != "Workspace") {
            // check that happens in laminar cabinet
            G.Instance.Progress.calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
            return;
        }
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        
        if (type == ObjectType.Syringe) {
            Syringe syringe = item as Syringe;
            if (syringe.Container.Capacity == 20) {
                EnableCondition("RightAmountInSyringe");
            }
        }

        bool check = CheckClearConditions(true);
        if (!check) {
            UISystem.Instance.CreatePopup(-1, "Wrong amount of medicine", MessageType.Mistake);
            G.Instance.Progress.calculator.Subtract(TaskType.MedicineToSyringe);
            base.FinishTask();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Medicine was successfully taken", MessageType.Notify);
        G.Instance.Progress.calculator.Add(TaskType.MedicineToSyringe);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Ota ruiskulla lääkettä lääkeainepullosta.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Valitse oikeankokoinen ruisku, jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula.";
    }
    #endregion
}