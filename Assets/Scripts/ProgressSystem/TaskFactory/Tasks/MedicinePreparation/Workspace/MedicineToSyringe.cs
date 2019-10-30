using System;
using System.Collections.Generic;
using UnityEngine;
public class MedicineToSyringe : TaskBase {
    #region Fields

    private Dictionary<int, int> syringes = new Dictionary<int, int>();
    public enum Conditions { RightAmountInSyringe }
    private CabinetBase laminarCabinet;
    private string description = "Ota ruiskulla lääkettä lääkeainepullosta.";
    private string hint = "Valitse oikeankokoinen ruisku (5ml), jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula.";
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for MedicineToSyringe task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public MedicineToSyringe() : base(TaskType.MedicineToSyringe, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 1;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        SubscribeEvent(AddSyringe, EventType.SyringeToMedicineBottle);
        SubscribeEvent(RemoveSyringe, EventType.SyringeFromMedicineBottle);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }
        
    }

    private void AddSyringe(CallbackData data) {
        Syringe s = data.DataObject as Syringe;
        syringes.Add(s.GetInstanceID(), s.Container.Amount);
    }

    private void RemoveSyringe(CallbackData data) {
        Syringe s = data.DataObject as Syringe;
        if (syringes.ContainsKey(s.GetInstanceID())) {
            if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
                G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
                UISystem.Instance.CreatePopup(-1, "Lääkettä otettiin laminaarikaapin ulkopuolella.", MessageType.Mistake);
            } else if (syringes[s.GetInstanceID()] != s.Container.Amount) {
                ToSyringe(s);
            } 
            syringes.Remove(s.GetInstanceID());
        }
    }

    /// <summary>
    /// Once fired by an event, checks which step of the MedicineToSyringe process has been taken and if required previous tasks are completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void ToSyringe(Syringe syringe) {
        if (syringe.Container.Amount == 900 && syringe.Container.Capacity == 5000) {
            EnableCondition(Conditions.RightAmountInSyringe);
        }

        bool check = CheckClearConditions(true);
        if (!check) {
            UISystem.Instance.CreatePopup(0, "Väärä ruiskun koko tai määrä lääkettä.", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.MedicineToSyringe);
            base.FinishTask();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Lääkkeen ottaminen onnistui.", MessageType.Notify);
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