using UnityEngine;

public class SelectTools : TaskBase {

    #region fields
    private string[] conditions = { "SyringePickedUp", "NeedlePickedUp", "LuerlockPickedUp" };
    #endregion

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
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            Logger.Print("Item was null");
            return;
        }

        ObjectType type = item.ObjectType;
        switch (type) {
            case ObjectType.Syringe:
                ToggleCondition("SyringePickedUp");
                break;
            case ObjectType.Needle:
                ToggleCondition("NeedlePickedUp");
                break;
            case ObjectType.Luerlock:
                ToggleCondition("LuerlockPickedUp");
                break;
        }
        CheckClearConditions(false);
    }
    #endregion

    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Tools Selected", MessageType.Notify);
        Logger.Print("All conditions fulfilled, task finished!");
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Valitse sopivat työvälineet.";
    }

    public override string GetHint() {
        return "Huoneessa on lääkkeen valmistukseen tarvittavia työvälineitä. Valitse oikea määrä tarvittavia välineitä.";
    }
}
