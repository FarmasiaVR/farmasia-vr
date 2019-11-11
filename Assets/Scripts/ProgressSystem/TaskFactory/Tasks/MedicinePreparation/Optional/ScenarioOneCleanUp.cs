using System;
using System.Collections.Generic;

/// <summary>
/// In case syringes were dropped, this task is created to check if the player puts the dropped syringes to trash before finishing the game.
/// </summary>
public class ScenarioOneCleanUp : TaskBase {
    #region Fields
    private string description = "Siivoa lopuksi työtila.";
    private string hint = "Vie pelin aikana lattialle pudonneet esineet roskakoriin.";
    private List<GeneralItem> itemsToBeCleaned;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ScenarioOneCleanUp task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public ScenarioOneCleanUp() : base(TaskType.ScenarioOneCleanUp, true, true) {
        Subscribe();
        points = 1;
        itemsToBeCleaned = new List<GeneralItem>();
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(ItemDroppedOnFloor, EventType.ItemDroppedOnFloor);
        base.SubscribeEvent(ItemLiftedOffFloor, EventType.ItemLiftedOffFloor);
        base.SubscribeEvent(ItemDroppedInTrash, EventType.ItemDroppedInTrash);
    }

    private void ItemDroppedOnFloor(CallbackData data) {
        if (G.Instance.Progress.IsCurrentPackage("Equipment Selection")) {
            return;
        }
        foreach (Package p in G.Instance.Progress.packages) {
            if (p.name == "Clean Up" && p.activeTasks.Count == 1) {
                p.AddNewTaskBeforeTask(this, p.activeTasks[0]);
                break;
            }
        }
        GeneralItem item = data.DataObject as GeneralItem;
        if (!itemsToBeCleaned.Contains(item)) {
            itemsToBeCleaned.Add(item);
        }
    }

    private void ItemLiftedOffFloor(CallbackData data) {
        if (G.Instance.Progress.IsCurrentPackage("Equipment Selection")) {
            return;
        }
        GeneralItem item = data.DataObject as GeneralItem;
        if (!item.IsClean && !G.Instance.Progress.IsCurrentPackage("Clean Up")) {
            UISystem.Instance.CreatePopup("Siivoa pudonneet työvälineet vasta lopuksi.", MsgType.Mistake);
        }
    }

    private void ItemDroppedInTrash(CallbackData data) {
        if (G.Instance.Progress.IsCurrentPackage("Equipment Selection")) {
            return;
        }
        GeneralItem item = data.DataObject as GeneralItem;
        if (!item.IsClean) {
            if (!G.Instance.Progress.IsCurrentPackage("Clean Up")) {
                UISystem.Instance.CreatePopup(-1, "Esine laitettiin roskakoriin liian aikaisin.", MsgType.Mistake);
                G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.ScenarioOneCleanUp);
            }
            itemsToBeCleaned.Remove(item);
        }
        if (itemsToBeCleaned.Count == 0 && base.package != null) {
            base.package.MoveTaskToManager(this);
        }
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (itemsToBeCleaned.Count != 0) {
            G.Instance.Progress.Calculator.Subtract(TaskType.ScenarioOneCleanUp);
        }
        base.FinishTask();
    }
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