using UnityEngine;
/// <summary>
/// Checks if Throughput Cupboard (läpiantokaappi) has correct layout.
/// </summary>
public class CorrectLayoutInThroughput : TaskBase {
    #region Fields
    private string[] conditions = { "AtLeastThree"/*, "ItemsArranged"*/ };
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectLayoutInThroughput task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectLayoutInThroughput() : base(TaskType.CorrectLayoutInThroughput, true, false) {
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
    /// Once fired by an event, checks how many items have been picked up and if they are arranged.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void ArrangeItems(CallbackData data) {
        Logger.Print(data.DataString + " counted!");
        int itemCount = int.Parse(data.DataString);
        if (itemCount >= 2) {
            EnableCondition("AtLeastThree");
            /*if (ItemsArranged()) {
                EnableCondition("ItemsArranged");
            }*/
        }
        bool check = CheckClearConditions(true);
        /*if (!check && AtLeastThree()) {
            UISystem.Instance.CreatePopup(-1, "Items not arranged", MessageType.Mistake);
            G.Instance.Progress.calculator.Subtract(TaskType.CorrectLayoutInThroughput);
            base.FinishTask();
        }*/
    }

    /// <summary>
    /// Checks that the items are arranged according to rules.
    /// </summary>
    /// <returns>"Returns true if the items are arranged."</returns>
    private bool ItemsArranged() {
        //code missing
        return false;
    }

    /// <summary>
    /// Sets the item count to be zero when exiting the room.
    /// </summary>
    private void InitializeCount() {
        //callback missing
        DisableConditions();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Items in order", MessageType.Notify);
        G.Instance.Progress.calculator.Add(TaskType.CorrectLayoutInThroughput);
        base.FinishTask();
    }

    /// <summary>
    /// This method is called if the task needs to be removed before progressing in game. 
    /// The task is removed without completion.
    /// </summary>
    public void RemoveTaskFromOutside() {
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