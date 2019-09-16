using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout1 : TaskBase {

    private string[] conditions = {"ItemsArranged"};
 
    public Layout1() : base(TaskType.Layout1, true, false) {
        Subscribe();
        AddConditions(conditions);
    }

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(ArrangeItems, EventType.ArrangeItems);
    }
    private void ArrangeItems(CallbackData data) {
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
        return "Asettele välineet läpiantokaappiin.";
    }

    public override string GetHint() {
        return "Vie valitsemasi työvälineet ja lääkepullo läpiantokaapin kautta toiseen huoneeseen."; 
    }
}