using UnityEngine;
public class SelectTools : TaskBase {

    private string[] conditions = {"SyringePickedUp", "NeedlePickedUp", "LuerlockPickedUp"};

    /// <summary>
    /// Constructor for SelectTools task. 
    /// Is removed when finished and doesn't require previous task to be done.
    /// </summary>
    public SelectTools() : base(TaskType.SelectTools, true, false) {
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
        ToggleCondition("LuerlockPickedUp");
        CheckClearConditions(false);
    }
    #endregion



    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Valitse sopivat työvälineet.";
    }

    public override string GetHint() {
        return "Huoneessa on lääkkeen valmistukseen tarvittavia välineitä. Valitse oikea määrä tarvittavia välineitä.";
    }
}
