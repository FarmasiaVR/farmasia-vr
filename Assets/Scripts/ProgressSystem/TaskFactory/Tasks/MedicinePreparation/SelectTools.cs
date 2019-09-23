using UnityEngine;
public class SelectTools : TaskBase {
    #region Fields
    private string[] conditions = { "SyringePickedUp", "LuerlockPickedUp" };
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor for SelectTools task. 
    /// Is removed when finished and doesn't require previous task to be done.
    /// </summary>
    public SelectTools() : base(TaskType.SelectTools, true, false) {
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
    /// Once fired by an event, checks if item is correct and sets corresponding condition to true.
    /// </summary>
    /// <param name="data">Reference to the GameObject that was picked up.</param>
    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;

        if (type == ObjectType.Syringe) {
            EnableCondition("SyringePickedUp");
        }
        if (type == ObjectType.Luerlock) {
            EnableCondition("LuerlockPickedUp");
        }
        CheckClearConditions(true);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions done, used for Finishing current task.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(0, "Tools Selected", MessageType.Congratulate);
        base.FinishTask();
    }

    /// <summary>
    /// Used for getting current task's description.
    /// </summary>
    /// <returns>Returns a String presentation of the description.</returns>
    public override string GetDescription() {
        return "Valitse sopivat työvälineet.";
    }

    /// <summary>
    /// Used for getting current task's hint.
    /// </summary>
    /// <returns>Returns a String presentation of the hint.</returns>
    public override string GetHint() {
        return "Huoneessa on lääkkeen valmistukseen tarvittavia työvälineitä. Valitse oikea määrä tarvittavia välineitä.";
    }
    #endregion
}
