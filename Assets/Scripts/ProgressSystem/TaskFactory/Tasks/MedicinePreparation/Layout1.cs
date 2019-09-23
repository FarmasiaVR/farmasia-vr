using UnityEngine;
public class Layout1 : TaskBase {
    #region Fields
    private string[] conditions = { "AtLeastThree", "ItemsArranged" };
    #endregion

    #region Constructor
    public Layout1() : base(TaskType.Layout1, true, false) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(ArrangeItems, EventType.ArrangeItems);
    }

    private void ArrangeItems(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (AtLeastThree()) {
            EnableCondition("AtLeastThree");
        }
        //checks that the items are arranged correctly
        EnableCondition("ItemsArranged");
        bool check = CheckClearConditions(true);
        if (!check && AtLeastThree()) {
            UISystem.Instance.CreatePopup(-1, "Items not arranged", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.Layout1);
            base.FinishTask();
        }
    }
    //checks that at least three items are placed before going through the door
    private bool AtLeastThree() {
        return false;
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Items in order", MessageType.Notify);
        G.Instance.Progress.Calculator.Add(TaskType.Layout1);
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Asettele välineet läpiantokaappiin.";
    }

    public override string GetHint() {
        return "Vie valitsemasi työvälineet ja lääkepullo läpiantokaapin kautta toiseen huoneeseen.";
    }
    #endregion
}