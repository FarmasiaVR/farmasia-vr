using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMedicine : TaskData  {

    private string[] conditions = {"RightBottleSize"};

    public SelectMedicine() : base(true, false) {
        Subscribe();
        AddConditions(conditions);
    }

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(PickupObject, EventType.PickupObject);
    }
    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        ToggleCondition("RightBottleSize");
        CheckClearConditions();
    }
    #endregion


    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        base.FinishTask();
    }

    public string GetDescription() {
        return "Valitse sopiva lääkepullo.";
    }

    public string GetHint() {
        return base.GetHint();
    }

    //public void Trigger() {
    //    throw new System.NotImplementedException();
    //}

    //public void NextTask() {
    //}

}
