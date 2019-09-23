using UnityEngine;
public class SelectMedicine : TaskBase {
    #region Fields
    private string[] conditions = { "BottlePickup" };
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes Select Medicine task. Is removed once done, and doesnt require previous task completion.
    /// </summary>
    public SelectMedicine() : base(TaskType.SelectMedicine, true, false) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(PickupObject, EventType.PickupObject);
    }

    /// <summary>
    /// Callback method for PickupObject trigger.
    /// </summary>
    /// <param name="data">Refers to data that the trigger returns</param>
    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Bottle) {
            EnableCondition("BottlePickup");
        }
        CheckClearConditions(true);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all tasks are finished, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(0, "Medicine selected", MessageType.Congratulate);
        base.FinishTask();
    }
    /// <summary>
    /// Used for getting current tasks description.
    /// </summary>
    /// <returns>Returns a String presentation of the description.</returns>
    public override string GetDescription() {
        return "Valitse sopiva lääkepullo.";
    }
    /// <summary>
    /// Used for getting the hint for current task.
    /// </summary>
    /// <returns>Returns a String presentation of the hint.</returns>
    public override string GetHint() {
        return "Jääkaapissa on erikokoisia lääkepulloja. Valitse näistä yksi.";
    }
    #endregion
}
