using System;
public class MedicineToSyringe : TaskBase {
    #region Fields

    public enum Conditions { RightAmountInSyringe }
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for MedicineToSyringe task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public MedicineToSyringe() : base(TaskType.MedicineToSyringe, true, true) {
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 1;
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
        if (!G.Instance.Progress.IsCurrentPackage("Workspace")) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
            UISystem.Instance.CreatePopup(-1, "Task tried before time", MessageType.Mistake);
            return;
        }
        // check that happens in laminar cabinet
        Syringe syringe = data.DataObject as Syringe;
        if (syringe == null) {
            return;
        }
        
        if (syringe.Container.Capacity == 20) {
            EnableCondition(Conditions.RightAmountInSyringe);
        }

        bool check = CheckClearConditions(true);
        if (!check) {
            UISystem.Instance.CreatePopup(-1, "Wrong amount of medicine, you need 20ml", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.MedicineToSyringe);
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