using System;
using System.Collections.Generic;
using UnityEngine;

public class Finish : TaskBase {
    #region Fields
    private CabinetBase laminarCabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for Finish task.
    ///  Is not removed when finished and requires previous task completion.
    ///  </summary>
    public Finish() : base(TaskType.Finish, false, true) {
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }        
    }
    #endregion

    #region Private Methods
    private void PointsForSmallSyringes() {
        int pointsForSyringeSize = 0;
        int pointsForMedicineAmount = 0;
        foreach (GameObject value in laminarCabinet.objectsInsideArea) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            if (type == ObjectType.Syringe) {
                Syringe s = item.GetComponent<Syringe>();
                if (s.Container.Capacity == 1000 && !s.hasBeenInBottle && s.Container.Amount > 0) {
                    pointsForSyringeSize = Math.Min(6, pointsForSyringeSize++);
                    if (s.Container.Amount == 150) {
                        pointsForMedicineAmount = Math.Min(6, pointsForMedicineAmount++);
                    }
                }
            }   
        }
        
        if (pointsForSyringeSize < 6) {
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.SyringeAttach, 6 - pointsForSyringeSize);
        }
        if (pointsForMedicineAmount < 6) {
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectAmountOfMedicineSelected, 6 - pointsForMedicineAmount);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all tasks are completed, this method is called.
    /// </summary>
    public override void FinishTask() {
        PointsForSmallSyringes();
        UISystem.Instance.CreatePopup("Onnittelut!\nKaikki tehtävät suoritettiin.", MessageType.Done);
        base.FinishTask();
    }

    public override string GetDescription() {
        return "Siirry pois työtilasta.";
    }

    public override string GetHint() {
        return base.GetHint();
    }
    #endregion
}
