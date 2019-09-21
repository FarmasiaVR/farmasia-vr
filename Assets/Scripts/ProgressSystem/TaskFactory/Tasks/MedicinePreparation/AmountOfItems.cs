using UnityEngine;

public class AmountOfItems : TaskBase {

    #region fields
    private string[] conditions = {"Syringe", "Needle", "Luerlock", "RightSizeBottle"};
    #endregion
 
    public AmountOfItems() : base(TaskType.AmountOfItems, true, false) {
        Subscribe();
        AddConditions(conditions);
    }

    #region Event Subscriptions
    public override void Subscribe() { 
        base.SubscribeEvent(Amount, EventType.AmountOfItems);
    }

    private void Amount(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            Logger.Print("AmountOfItems: item was null");
            return;
        }

        ObjectType type = item.ObjectType;
        switch (type) {
            case ObjectType.Syringe:
                ToggleCondition("Syringe");
                break;
            case ObjectType.Needle:
                ToggleCondition("Needle");
                break;
            case ObjectType.Luerlock:
                ToggleCondition("Luerlock");
                break;
            case ObjectType.Bottle:
                //check that the chosen bottle is the wanted size
                ToggleCondition("RightSizeBottle");
                break;
        }

        bool check = CheckClearConditions(true);
        //checked when player exits the room
        if (!check) {
            Logger.Print("All conditions not fulfilled but task closed.");
            ProgressManager.Instance.GetCalculator().Substract(TaskType.AmountOfItems);
            base.FinishTask();
            ProgressManager.Instance.AddTask(TaskFactory.GetTask(TaskType.MissingItems));
        }
    }
    #endregion

    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        ProgressManager.Instance.GetCalculator().Add(TaskType.AmountOfItems);
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Tarkista valitsemiesi välineiden määrä.";
    }

    public override string GetHint() {
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla."; 
    }
}