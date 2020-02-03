using System;
using System.Collections.Generic;
using UnityEngine;

// Class is deprecated
public class CorrectLayoutInLaminarCabinet : TaskBase {
    #region Fields
    private CabinetBase laminarCabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectLayoutInLaminarCabinet task.
    ///  Is moved to manager when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectLayoutInLaminarCabinet() : base(TaskType.CorrectLayoutInLaminarCabinet, false, false) {
        base.unsubscribeAllEvents = false;
        Subscribe();
        points = 0;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        SubscribeEvent(VentilationBlocked, EventType.VentilationBlocked);
        SubscribeEvent(ArrangedItems, EventType.CorrectLayoutInLaminarCabinet);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    /// <summary>
    /// Checks if items have been arranged inside Laminar Cabinet.
    /// </summary>
    /// <param name="data"></param>
    private void ArrangedItems(CallbackData data) {
        if (laminarCabinet == null) {
            return;
        }
        if (!ItemsArranged()) {
            UISystem.Instance.CreatePopup(0, "Työvälineitä ei ryhmitelty.", MsgType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.CorrectLayoutInLaminarCabinet);
        }
    }

    private void VentilationBlocked(CallbackData data) {
        UISystem.Instance.CreatePopup(0, "Ilmanvaihto estynyt.", MsgType.Mistake);
        G.Instance.Progress.Calculator.Subtract(TaskType.CorrectLayoutInLaminarCabinet);
    }

    /// <summary>
    /// Checks that the items are arranged according to rules.
    /// </summary>
    /// <returns>"Returns true if the items are arranged."</returns>
    private bool ItemsArranged() {
        //code missing
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