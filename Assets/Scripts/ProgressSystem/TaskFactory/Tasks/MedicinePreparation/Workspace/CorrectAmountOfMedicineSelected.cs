using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of medicine pulled into smaller syringes through LuerLock.
/// </summary>
public class CorrectAmountOfMedicineSelected : TaskBase {
    #region Fields
    public enum Conditions { RightAmountOfMedicine }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.MedicineToSyringe, TaskType.LuerlockAttach };
    private Dictionary<int, int> attachedSyringes = new Dictionary<int, int>();
    private CabinetBase laminarCabinet;
    private string description = "Vedä ruiskuun lääkettä.";
    private string hint = "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.";
    private const int MinimumCorrectAmountInSmallSyringe = 140;
    private const int MaximumCorrectAmountInSmallSyringe = 160;
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
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
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
                    if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
                        G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectAmountOfMedicineSelected);
                        UISystem.Instance.CreatePopup(-1, "Lääkettä otettiin laminaarikaapin ulkopuolella.", MsgType.Mistake);
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
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        if (attachedSyringes.Count != 6) {
            return;
        }

        int rightAmount = 0;
        foreach (var amount in attachedSyringes.Values) {
            if (amount >= MinimumCorrectAmountInSmallSyringe && amount <= MaximumCorrectAmountInSmallSyringe) {
                rightAmount++;
            }
        }
        if (rightAmount == 6) {
            UISystem.Instance.CreatePopup("Valittiin oikean määrä lääkettä.", MsgType.Notify);
        } else {
            UISystem.Instance.CreatePopup("Yhdessä tai useammassa ruiskussa oli väärä määrä lääkettä.", MsgType.Notify);
        }
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