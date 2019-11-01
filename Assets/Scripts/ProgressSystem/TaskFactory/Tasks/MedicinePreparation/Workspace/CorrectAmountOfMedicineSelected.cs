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
    private int smallSyringes;
    private int rightAmountInSyringes;
    private CabinetBase laminarCabinet;
    private string description = "Vedä ruiskuun lääkettä.";
    private string hint = "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.";
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectAmountOfMedicineSelected task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public CorrectAmountOfMedicineSelected() : base(TaskType.CorrectAmountOfMedicineSelected, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        smallSyringes = 0;
        rightAmountInSyringes = 0;
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

        attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);
    }

    private void RemoveSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();

        if (attachedSyringes.ContainsKey(s.GetInstanceID())) {
            if (attachedSyringes[s.GetInstanceID()] != s.Container.Amount) {
                if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
                    G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectAmountOfMedicineSelected);
                    UISystem.Instance.CreatePopup(-1, "Lääkettä otettiin laminaarikaapin ulkopuolella.", MessageType.Mistake);
                } else {
                    Medicine(s);
                }
            }
            attachedSyringes.Remove(s.GetInstanceID());

            foreach (ITask task in G.Instance.Progress.GetAllTasks()) {
                if (task.GetTaskType() == TaskType.SyringeAttach) {
                    base.package.MoveTaskFromManagerBeforeTask(TaskType.SyringeAttach, this);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// Once fired by an event, checks if right amount has been chosen and if required previous tasks are completed.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void Medicine(Syringe syringe) {
        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            return;
        }
        CheckSyringe(syringe);
        if (rightAmountInSyringes == 6) {
            EnableCondition(Conditions.RightAmountOfMedicine);
        }

        if (smallSyringes == 6) {
            bool check = CheckClearConditions(true);
            if (!check) {
                UISystem.Instance.CreatePopup(rightAmountInSyringes, "Lääkettä otettiin väärä määrä.", MessageType.Mistake);
                G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectAmountOfMedicineSelected, smallSyringes - rightAmountInSyringes);
                base.FinishTask();
            }
        }
    }

    private void CheckSyringe(Syringe syringe) {
        if (syringe.Container.Capacity == 1000) {
            smallSyringes++;
        }
        if (syringe.Container.Amount == 150) {
            rightAmountInSyringes++;
        }  
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(6, "Lääkettä otettiin tarvittava määrä.", MessageType.Notify);
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