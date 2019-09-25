using UnityEngine;
public class Layout1 : TaskBase {
    #region Fields
    private string[] conditions = { "AtLeastThree", "ItemsArranged" };
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for Layout1 task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public Layout1() : base(TaskType.Layout1, true, false) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(ArrangeItems, EventType.ArrangeItems);
    }
    /// <summary>
    /// Once fired by and event, checks how many items have been picked up and if they are arranged.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
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
    /// <summary>
    /// Checks that at least three items are placed.
    /// </summary>
    /// <returns>"Returns true if at least three items are found."</returns>
    private bool AtLeastThree() {
        return false;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Items in order", MessageType.Notify);
        G.Instance.Progress.Calculator.Add(TaskType.Layout1);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Asettele välineet läpiantokaappiin.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Vie valitsemasi työvälineet ja lääkepullo läpiantokaapin kautta toiseen huoneeseen.";
    }
    #endregion
}