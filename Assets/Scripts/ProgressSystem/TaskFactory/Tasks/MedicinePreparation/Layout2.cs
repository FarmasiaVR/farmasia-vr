using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Layout2 : TaskBase {

    #region fields
    private string[] conditions = {"AllItems", "ItemsArranged"};
    #endregion
 
    public Layout2() : base(TaskType.Layout2, true, false) {
        Subscribe();
        AddConditions(conditions);
    }

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(FinalArrangeItems, EventType.FinalArrangeItems);
    }

    private void FinalArrangeItems(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        if (ProgressManager.Instance.GetDoneTaskTypes().Contains(TaskType.AmountOfItems)) {
            List<ITask> list = ProgressManager.Instance.GetActiveTasks();
            int exists = 0;
            exists = (from n in list
                    where n.GetTaskType().Equals(TaskType.MissingItems)
                    select n).Count();
            if (exists == 0) {
                ToggleCondition("AllItems"); 
            } 
        }
        //checks before moving to the next task if any of the items is on the prohibited area or on top of each other
        ToggleCondition("ItemsArranged");
        bool check = CheckClearConditions(true);
        if (!check) {
            Logger.Print("All conditions not fulfilled but task closed.");
            ProgressManager.Instance.GetCalculator().Substract(TaskType.Layout2);
            base.FinishTask();
        }
    }
    #endregion

    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        ProgressManager.Instance.GetCalculator().Add(TaskType.Layout2);
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Siirrä välineet läpiantokaapista kaappiin.";
    }

    public override string GetHint() {
        return "Vie ja asettele valitsemasi työvälineet sekä lääkepullo läpiantokaapista kaappiin."; 
    }
}