using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout2 : TaskBase {

    private string[] conditions = {"ItemsArranged"};
 
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
        ToggleCondition("ItemsArranged");
        CheckClearConditions(true);
    }
    #endregion

    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Siirrä välineet läpiantokaapista kaappiin.";
    }

    public override string GetHint() {
        return "Vie ja asettele valitsemasi työvälineet sekä lääkepullo läpiantokaapista kaappiin."; 
    }
}