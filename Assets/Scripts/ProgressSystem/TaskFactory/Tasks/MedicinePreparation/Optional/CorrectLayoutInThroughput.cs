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
        points = 0;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(ArrangedItems, EventType.CorrectLayoutInThroughput);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.PassThrough) {
            this.cabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    /// <summary>
    /// Once fired by an event, checks how many items have been picked up and if they are arranged.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void ArrangedItems(CallbackData data) {
        if (!ItemsArranged()) {
            UISystem.Instance.CreatePopup(0, "Työvälineitä ei ryhmitelty.", MsgType.Mistake);
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
    public override void FinishTask() {
        base.FinishTask();
    }

    public override string GetDescription() {
        return "";
    }

    public override string GetHint() {
        return "";
    }

    protected override void OnTaskComplete() {
        //throw new NotImplementedException();
    }
    #endregion
}