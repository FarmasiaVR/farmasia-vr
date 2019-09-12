using UnityEngine;
public class SelectTools2 : TaskBase {

    private string[] conditions = {"SyringePickedUp", "NeedlePickedUp"};

    /// <summary>
    /// Constructor for SelectTools task. 
    /// Is removed when finished and doesn't require previous task to be done.
    /// </summary>
    public SelectTools2() : base(true, false) {
        Subscribe();
        AddConditions(conditions);
    }

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(PickupObject, EventType.PickupObject);
    }
    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        ToggleCondition("SyringePickedUp");
        ToggleCondition("NeedlePickedUp");
        CheckClearConditions();
    }
    #endregion



    public override void FinishTask() {
        Logger.Print("All conditions done! - Task finished!");
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Valitse sopiva määrä välineitä.";
    }

    public override string GetHint() {
        return base.GetHint();
    }
}
