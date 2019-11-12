using System;
using System.Collections.Generic;
using UnityEngine;
public class MedicineToSyringe : TaskBase {
    #region Fields
    private Dictionary<int, int> syringes = new Dictionary<int, int>();
    public enum Conditions { RightSize, RightAmountInSyringe }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.CorrectItemsInLaminarCabinet };
    private CabinetBase laminarCabinet;
    private string description = "Ota ruiskulla lääkettä lääkeainepullosta.";
    private string hint = "Valitse oikeankokoinen ruisku (20ml), jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula.";
    #endregion

    #region States
    private bool takenBeforeTime = false;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for MedicineToSyringe task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public MedicineToSyringe() : base(TaskType.MedicineToSyringe, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
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
        if (!syringes.ContainsKey(s.GetInstanceID())) {
            syringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
        if (!CheckPreviousTaskCompletion(requiredTasks) && G.Instance.Progress.CurrentPackage.name == PackageName.Workspace) {
            UISystem.Instance.CreatePopup("Siirrä kaikki tarvittavat työvälineet ensin laminaarikaappiin.", MsgType.Notify);
        }
    }

    private void RemoveSyringe(CallbackData data) {
        Syringe s = data.DataObject as Syringe;
        if (syringes.ContainsKey(s.GetInstanceID())) {
            if (laminarCabinet == null && CheckSyringeLiquidChange(s)) {
                if (!takenBeforeTime) {
                    G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
                    UISystem.Instance.CreatePopup(-1, "Lääkettä yritettiin ottaa liian aikaisin.", MsgType.Mistake);
                    takenBeforeTime = true;
                } else {
                    UISystem.Instance.CreatePopup("Lääkettä yritettiin ottaa liian aikaisin.", MsgType.Mistake);
                }
            } else if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
                G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
                UISystem.Instance.CreatePopup(-1, "Lääkettä yritettiin ottaa laminaarikaapin ulkopuolella.", MsgType.Mistake);
            } else if (CheckSyringeLiquidChange(s)) {
                if (!CheckPreviousTaskCompletion(requiredTasks)) {
                    foreach (ITask task in G.Instance.Progress.CurrentPackage.activeTasks) {
                        if (task.GetTaskType() == TaskType.CorrectItemsInLaminarCabinet) {
                            task.UnsubscribeAllEvents();
                            task.RemoveFromPackage();
                            break;
                        }
                    }

                    UISystem.Instance.CreatePopup(-1, "Tarvittavia työvälineitä ei siirretty laminaarikaappiin.", MsgType.Mistake);
                    G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectItemsInLaminarCabinet);
                }
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
        if (syringe.Container.Amount >= 900) {
            EnableCondition(Conditions.RightAmountInSyringe);
        }
        if (syringe.Container.Capacity == 20000) {
            EnableCondition(Conditions.RightSize);
        }

        bool check = CheckClearConditions(true);
        if (!check) {
            ReceivedPoints();
            base.FinishTask();
        }
    }

    private bool CheckSyringeLiquidChange(Syringe syringe) {
        if (syringes[syringe.GetInstanceID()] != syringe.Container.Amount) {
            return true;
        }
        return false;
    }

    private void ReceivedPoints() {
        if (base.GetNonClearedConditions().Count == 2) {
            UISystem.Instance.CreatePopup(0, "Väärä ruiskun koko ja määrä lääkettä.", MsgType.Mistake);
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.MedicineToSyringe, 2);
        } else {
            UISystem.Instance.CreatePopup(1, "Väärä ruiskun koko tai määrä lääkettä.", MsgType.Mistake);
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.MedicineToSyringe, 1);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(2, "Lääkkeen ottaminen onnistui.", MsgType.Notify);
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