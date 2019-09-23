using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingItems : TaskBase {

    private string[] conditions = {"Syringe", "Needle", "Luerlock", "RightSizeBottle"};
 
    public MissingItems() : base(TaskType.MissingItems, true, false) {
        Subscribe();
        AddConditions(conditions);
    }

    #region Event Subscriptions
    public override void Subscribe() { 
        base.SubscribeEvent(Missing, EventType.Missing);
    }
    private void Missing(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        
        if (type == ObjectType.Syringe) {
            EnableCondition("Syringe");
        }
        if (type == ObjectType.Needle) {
            EnableCondition("Needle");
        }
        if (type == ObjectType.Luerlock) {
            EnableCondition("Luerlock");
        }
        if (type == ObjectType.Bottle) {
            //check that the chosen bottle has the wanted size
            EnableCondition("RightSizeBottle");
        }
        CheckClearConditions(true);
    }
    #endregion 

    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Missing items picked up", MessageType.Done);
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Hae puuttuvat työvälineet.";
    }

    public override string GetHint() {
        return "Ensimmäisellä hakukerralla osa tarvittavista työvälineistä jäi valitsematta. Palaa ensimmäiseen huoneeseen hakemaan puuttuvat tavarat."; 
    }
}