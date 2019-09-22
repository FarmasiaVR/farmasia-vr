using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout2 : TaskBase {
    #region Fields
    private string[] conditions = {"ItemsArranged"};
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
        EnableCondition("ItemsArranged");
        CheckClearConditions(true);
    }
    #endregion

    #region Public Methods
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
    #endregion
}