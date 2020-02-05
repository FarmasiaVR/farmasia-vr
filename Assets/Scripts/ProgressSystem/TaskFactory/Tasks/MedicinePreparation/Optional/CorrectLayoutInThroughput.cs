using System;
using UnityEngine;
/// <summary>
/// Checks if Throughput Cupboard (läpiantokaappi) has correct layout.
/// </summary>

    // Class is deprecated
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
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.PassThrough) {
            this.cabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
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