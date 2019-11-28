using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of medicine pulled into smaller syringes through LuerLock.
/// </summary>
public class CorrectAmountOfMedicineSelected : TaskBase {

    #region Constants
    private const int MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 140;
    private const int MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 160;

    private const string DESCRIPTION = "Vedä ruiskuun lääkettä.";
    private const string HINT = "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.";
    #endregion

    #region Fields
    public enum Conditions { RightAmountOfMedicine }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.MedicineToSyringe, TaskType.LuerlockAttach };
    private Dictionary<int, int> attachedSyringes = new Dictionary<int, int>();
    private CabinetBase laminarCabinet;
    
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectAmountOfMedicineSelected task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public CorrectAmountOfMedicineSelected() : base(TaskType.CorrectAmountOfMedicineSelected, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 6;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        base.SubscribeEvent(AddSyringe, EventType.SyringeToLuerlock);
        base.SubscribeEvent(RemoveSyringe, EventType.SyringeFromLuerlock);
    }
    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }
    }

    private void AddSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();
        if (!attachedSyringes.ContainsKey(s.GetInstanceID()) && !s.hasBeenInBottle) {
            attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
    }

    private void RemoveSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();
 
        if (attachedSyringes.ContainsKey(s.GetInstanceID())) {
            if (CheckPreviousTaskCompletion(requiredTasks)) {
                if (attachedSyringes[s.GetInstanceID()] != s.Container.Amount) {
                    attachedSyringes[s.GetInstanceID()] = s.Container.Amount;

                    if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
                        G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectAmountOfMedicineSelected);
                        UISystem.Instance.CreatePopup(-1, "Lääkettä otettiin laminaarikaapin ulkopuolella.", MsgType.Mistake);
                        AudioManager.Instance?.Play("mistakeMessage");
                        attachedSyringes.Remove(s.GetInstanceID());
                    } else if (attachedSyringes.Count != 6) {
                        if (s.Container.Amount >= MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && s.Container.Amount <= MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
                            UISystem.Instance.CreatePopup("Ruiskuun otettiin oikea määrä lääkettä.", MsgType.Notify);
                        } else {
                            UISystem.Instance.CreatePopup("Ruiskuun otettiin väärä määrä lääkettä.", MsgType.Notify);
                            AudioManager.Instance?.Play("mistakeMessage");
                        }
                    } else {
                        FinishTask();
                    }
                }
            } else {
                attachedSyringes.Remove(s.GetInstanceID());
            }

            foreach (ITask task in G.Instance.Progress.GetAllTasks()) {
                if (task.GetTaskType() == TaskType.SyringeAttach) {
                    base.package.MoveTaskFromManagerBeforeTask(TaskType.SyringeAttach, this);
                    break;
                }
            }
        }
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        int rightAmount = 0;
        foreach (var amount in attachedSyringes.Values) {
            if (amount >= MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && amount <= MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
                rightAmount++;
            }
        }
        if (rightAmount == 6) {
            UISystem.Instance.CreatePopup("Valittiin oikea määrä lääkettä.", MsgType.Notify);
        } else {
            UISystem.Instance.CreatePopup("Yhdessä tai useammassa ruiskussa oli väärä määrä lääkettä.", MsgType.Notify);
        }
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