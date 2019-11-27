using System;
using System.Collections.Generic;
using UnityEngine;
public class MedicineToSyringe : TaskBase {

    #region Constants
    private const int RIGHT_SYRINGE_CAPACITY = 20000;
    private const int MINIMUM_AMOUNT_OF_MEDICINE_IN_BIG_SYRINGE = 900;

    private const string DESCRIPTION = "Ota ruiskulla lääkettä lääkeainepullosta.";
    private const string HINT = "Valitse oikeankokoinen ruisku (20ml), jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula.";
    #endregion

    #region Fields
    private Dictionary<int, int> syringes = new Dictionary<int, int>();
    public enum Conditions { RightSize, RightAmountInSyringe }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.CorrectItemsInLaminarCabinet };
    private CabinetBase laminarCabinet;
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
            AudioManager.Instance.Play("mistakeMessage");
        }
    }

    private void RemoveSyringe(CallbackData data) {
        Syringe s = data.DataObject as Syringe;
        if (syringes.ContainsKey(s.GetInstanceID())) {
            if (LiquidAmountHasChanged(s)) {
                if (laminarCabinet == null) {
                    if (!takenBeforeTime) {
                        G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
                        UISystem.Instance.CreatePopup(-1, "Lääkettä yritettiin ottaa liian aikaisin.", MsgType.Mistake);
                        AudioManager.Instance.Play("mistakeMessage");
                        takenBeforeTime = true;
                    } else {
                        UISystem.Instance.CreatePopup("Lääkettä yritettiin ottaa liian aikaisin.", MsgType.Mistake);
                        AudioManager.Instance.Play("mistakeMessage");
                    }
                } else if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
                    G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
                    UISystem.Instance.CreatePopup(-1, "Lääkettä yritettiin ottaa laminaarikaapin ulkopuolella.", MsgType.Mistake);
                    AudioManager.Instance.Play("mistakeMessage");
                } else {
                    if (!CheckPreviousTaskCompletion(requiredTasks)) {
                        foreach (ITask task in G.Instance.Progress.CurrentPackage.activeTasks) {
                            if (task.GetTaskType() == TaskType.CorrectItemsInLaminarCabinet) {
                                task.UnsubscribeAllEvents();
                                task.RemoveFromPackage();
                                break;
                            }
                        }

                        UISystem.Instance.CreatePopup(-1, "Tarvittavia työvälineitä ei siirretty laminaarikaappiin.", MsgType.Mistake);
                        AudioManager.Instance.Play("mistakeMessage");
                        G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectItemsInLaminarCabinet);
                    }
                    CheckConditions(s);
                }
            }
            syringes.Remove(s.GetInstanceID());
        }
    }

    private void CheckConditions(Syringe syringe) {
        if (syringe.Container.Amount >= MINIMUM_AMOUNT_OF_MEDICINE_IN_BIG_SYRINGE) {
            EnableCondition(Conditions.RightAmountInSyringe);
        }
        if (syringe.Container.Capacity == RIGHT_SYRINGE_CAPACITY) {
            EnableCondition(Conditions.RightSize);
        }

        if (!CheckClearConditions(true)) {
            ReceivedPoints();
            base.FinishTask();
        }
    }

    private bool LiquidAmountHasChanged(Syringe syringe) {
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
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(2, "Lääkkeen ottaminen onnistui.", MsgType.Notify);
        base.FinishTask();
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }
    #endregion
}