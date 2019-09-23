using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout2 : TaskBase {
    #region Fields
    private string[] conditions = {"AllItems", "ItemsArranged"};
    #endregion

    #region Constructor
    public Layout2() : base(TaskType.Layout2, true, false) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(FinalArrangeItems, EventType.FinalArrangeItems);
    }

    private void FinalArrangeItems(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        if (G.Instance.Progress.DoneTypes.Contains(TaskType.AmountOfItems)) {
            List<ITask> list = G.Instance.Progress.ActiveTasks;
            int exists = 0;
            exists = (from n in list
                    where n.GetTaskType().Equals(TaskType.MissingItems)
                    select n).Count();
            if (exists == 0) {
                EnableCondition("AllItems"); 
            } 
        }
        //checks before moving to the next task if any of the items is on the prohibited area or on top of each other
        EnableCondition("ItemsArranged");
        bool check = CheckClearConditions(true);
        if (!check) {
            Logger.Print("All conditions not fulfilled but task closed.");
            G.Instance.Progress.Calculator.Subtract(TaskType.Layout2); 
            base.FinishTask();
        }
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        G.Instance.Progress.Calculator.Add(TaskType.Layout2);
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Siirrä välineet läpiantokaapista kaappiin.";
    }

    public override string GetHint() {
        return "Vie ja asettele valitsemasi työvälineet sekä lääkepullo läpiantokaapista kaappiin."; 
    }
    #endregion
}