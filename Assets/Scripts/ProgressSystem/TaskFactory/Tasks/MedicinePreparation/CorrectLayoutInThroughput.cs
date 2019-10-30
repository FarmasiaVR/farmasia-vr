using System;
using UnityEngine;
/// <summary>
/// Checks if Throughput Cupboard (läpiantokaappi) has correct layout.
/// </summary>
public class CorrectLayoutInThroughput : TaskBase {
    #region Fields
    private CabinetBase cabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectLayoutInThroughput task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectLayoutInThroughput() : base(TaskType.CorrectLayoutInThroughput, true, false) {
        Subscribe();
        points = 1;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(ArrangedItems, EventType.CorrectLayoutInThroughput);
    }

    /// <summary>
    /// Once fired by an event, checks how many items have been picked up and if they are arranged.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void ArrangedItems(CallbackData data) {
        if (ItemsArranged()) {
            UISystem.Instance.CreatePopup(-1, "Työvälineitä ei ryhmitelty.", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.CorrectLayoutInThroughput);
        }
    }

    /// <summary>
    /// Checks that the items are arranged according to rules.
    /// </summary>
    /// <returns>"Returns true if the items are arranged."</returns>
    private bool ItemsArranged() {
        /* KESKEN
        List<GameObject> objects = cabinet.GetContainedItems();
        Collider collisionMashUp = null;
        foreach (GameObject leobject in objects) {
            Collider col = leobject.GetComponent<Collider>();
            
            if (collisionMashUp == null) {
                collisionMashUp = col;
                continue;
            }
            
            if () {
                return false;
            }
            
        }*/
        return true;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        base.FinishTask();
    }

    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "";
    }
    #endregion
}