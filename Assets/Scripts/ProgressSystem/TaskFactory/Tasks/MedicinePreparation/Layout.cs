using UnityEngine;
public class Layout : TaskBase {

    private string[] conditions = {"SyringePickedUp", "NeedlePickedUp", "LuerlockPickedUp", "BottlePickedUp"};

    /// <summary>
    /// Constructor for SelectTools task. 
    /// Is removed when finished and doesn't require previous task to be done.
    /// </summary>
    public Layout() : base(true, true) {
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
        Logger.Print("All conditions fullfilled, task completed.");
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Asettele välineet läpiantokaappiin.";
    }

    public override string GetHint() {
        return base.GetHint();
    }
}