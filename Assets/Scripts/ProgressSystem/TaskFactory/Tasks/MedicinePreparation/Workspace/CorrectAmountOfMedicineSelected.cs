using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of medicine pulled into smaller syringes through LuerLock.
/// </summary>
public class CorrectAmountOfMedicineSelected : TaskBase {

    #region Constants
    private const int MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 150;
    private const int MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 150;

    private const string DESCRIPTION = "Vedä ruiskuun lääkettä.";
    private const string HINT = "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.";
    #endregion

    #region Fields
    public enum Conditions { }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.MedicineToSyringe, TaskType.LuerlockAttach };
    private Dictionary<int, int> attachedSyringes = new Dictionary<int, int>();
    private CabinetBase laminarCabinet;

    private Dictionary<Syringe, int> usedSyringes;
    #endregion

    #region Constructor
    public CorrectAmountOfMedicineSelected() : base(TaskType.CorrectAmountOfMedicineSelected, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        usedSyringes = new Dictionary<Syringe, int>();
        points = 6;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(AddSyringe, EventType.SyringeToLuerlock);
        base.SubscribeEvent(RemoveSyringe, EventType.SyringeFromLuerlock);
        base.SubscribeEvent(InvalidSyringePush, EventType.PushingToSmallerSyringe);
    }
    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void AddSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();

        usedSyringes.Add(s, 0);

        if (!attachedSyringes.ContainsKey(s.GetInstanceID()) && !s.hasBeenInBottle) {
            attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
    }

    private void RemoveSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();

        int minus = 0;
        int oldMinus = 0;

        if (usedSyringes.ContainsKey(s)) {
            oldMinus = usedSyringes[s];
        }

        if (!s.IsClean) {
            minus--;
            Popup("Ruisku tai luerlock oli likainen", MsgType.Mistake, -1);
        }
        if (s.Container.Amount != MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
            minus--;
            Popup("Väärä määrä lääkettä ruiskussa", MsgType.Mistake, -1);
        } else {
            Popup("Ruiskuun otettiin oikea määrä lääkettä.", MsgType.Done);
        }

        if (usedSyringes.Count >= 6) {
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectAmountOfMedicineSelected, minus - oldMinus);
            CompleteTask();
        }

        //if (attachedSyringes.ContainsKey(s.GetInstanceID())) {
        //    if (IsPreviousTasksCompleted(requiredTasks)) {
        //        if (attachedSyringes[s.GetInstanceID()] != s.Container.Amount) {
        //            attachedSyringes[s.GetInstanceID()] = s.Container.Amount;

        //            if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
        //                G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectAmountOfMedicineSelected);
        //                Popup("Lääkettä otettiin laminaarikaapin ulkopuolella.", MsgType.Mistake, -1);
        //                attachedSyringes.Remove(s.GetInstanceID());
        //            } else if (attachedSyringes.Count != 6) {
        //                if (s.Container.Amount >= MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && s.Container.Amount <= MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
        //                    Popup("Ruiskuun otettiin oikea määrä lääkettä.", MsgType.Notify);
        //                } else {
        //                    Popup("Ruiskuun otettiin väärä määrä lääkettä.", MsgType.Mistake);
        //                }
        //            } else {
        //                CompleteTask();
        //                return;
        //            }
        //        }
        //    } else {
        //        attachedSyringes.Remove(s.GetInstanceID());
        //    }

        //    foreach (ITask task in G.Instance.Progress.GetAllTasks()) {
        //        if (task.GetTaskType() == TaskType.SyringeAttach) {
        //            package.MoveTaskFromManagerBeforeTask(TaskType.SyringeAttach, this);
        //            break;
        //        }
        //    }
        //}
    }

    private void InvalidSyringePush(CallbackData data) {
        G.Instance.Progress.Calculator.Subtract(TaskType.CorrectAmountOfMedicineSelected);
        Popup("Älä työnnä isosta ruiskusta pieneen. Vedä pienellä.", MsgType.Mistake, -1);
    }
    #endregion

    #region Public Methods

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }

    protected override void OnTaskComplete() {
        int rightAmount = 0;
        foreach (var amount in attachedSyringes.Values) {
            if (amount >= MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && amount <= MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
                rightAmount++;
            }
        }
        if (rightAmount == 6) {
            Popup("Valittiin oikea määrä lääkettä.", MsgType.Notify);
        } else {
            Popup("Yhdessä tai useammassa ruiskussa oli väärä määrä lääkettä.", MsgType.Notify);
        }
    }
    #endregion
}