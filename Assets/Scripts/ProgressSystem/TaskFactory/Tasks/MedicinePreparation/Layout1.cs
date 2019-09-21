using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout1 : TaskBase {

    private string[] conditions = {"AtLeastThree", "ItemsArranged"};
 
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
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            Logger.Print("was null");
            return;
        }
        ObjectType type = item.ObjectType;
        if (AtLeastThree()) {
            ToggleCondition("AtLeastThree");
        }
        //checks that the items are arranged correctly
        ToggleCondition("ItemsArranged");
        bool check = CheckClearConditions(true);
        if (!check && AtLeastThree()) {
            Logger.Print("All conditions not fulfilled but task closed.");
            ProgressManager.Instance.GetCalculator().Substract(TaskType.Layout1);
            base.FinishTask();
        }
    }
   //checks that at least three items are placed before going through the door
    private bool AtLeastThree() {
        return false;
    }
    #endregion

    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        ProgressManager.Instance.GetCalculator().Add(TaskType.Layout1);
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Asettele välineet läpiantokaappiin.";
    }

    public override string GetHint() {
        return "Vie valitsemasi työvälineet ja lääkepullo läpiantokaapin kautta toiseen huoneeseen."; 
    }
}