using System;

/// <summary>
/// In case syringes were dropped, this task is created to check if the player puts the dropped syringes to trash before finishing the game.
/// </summary>
public class ScenarioOneCleanUp : TaskBase {
    #region Fields
    public enum Conditions { DroppedItemsPutToTrash, PreviousTasksCompleted }
    private string description = "Siivoa lopuksi työtila.";
    private string hint = "Vie pelin aikana lattialle pudonneet esineet roskakoriin.";
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ScenarioOneCleanUp task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public ScenarioOneCleanUp() : base(TaskType.ScenarioOneCleanUp, true, true) {
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 1;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(CleanUp, EventType.CleanUp);
        base.SubscribeEvent(ItemDroppedInTrash, EventType.ItemDroppedInTrash);
        base.SubscribeEvent(ItemLiftedOffFloor, EventType.ItemLiftedOffFloor);
    }
    /// <summary>
    /// Once fired by an event, checks if dropped syringes are put to trash and if required previous tasks are completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void CleanUp(CallbackData data) {
        bool allDroppedItemsInTrash = data.DataBoolean;
        if (G.Instance.Progress.IsCurrentPackage("Clean up")) {
            EnableCondition(Conditions.PreviousTasksCompleted);
        }

        if (allDroppedItemsInTrash) {
            EnableCondition(Conditions.DroppedItemsPutToTrash);
        }

        bool check = CheckClearConditions(true);
        if (!check) {
            UISystem.Instance.CreatePopup(-1, "Välineitä ei viety roskakoriin.", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.ScenarioOneCleanUp);
            base.FinishTask();
        }
    }

    private void ItemDroppedInTrash(CallbackData data) {
        GeneralItem item = data.DataObject as GeneralItem;
        if (!item.IsClean && !G.Instance.Progress.IsCurrentPackage("Clean up")) {
            UISystem.Instance.CreatePopup("Esineet laitettiin roskakoriin liian aikaisin.", MessageType.Mistake);
            base.UnsubscribeEvent(ItemDroppedInTrash, EventType.ItemLiftedOffFloor);
        }
    }

    private void ItemLiftedOffFloor(CallbackData data) {
        GeneralItem item = data.DataObject as GeneralItem;
        if (!item.IsClean && !G.Instance.Progress.IsCurrentPackage("Clean up")) {
            UISystem.Instance.CreatePopup("Siivosit liian aikaisin.", MessageType.Mistake);
            base.UnsubscribeEvent(ItemLiftedOffFloor, EventType.ItemLiftedOffFloor);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return description;
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return hint;
    }
    #endregion
}