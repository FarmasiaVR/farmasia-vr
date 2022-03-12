using System;
using System.Collections.Generic;
using UnityEngine;
public class SyringeAttach : Task {

    #region Constants
    private const int RIGHT_SMALL_SYRINGE_CAPACITY = 1000;

    public override string Description { get => "Yhdistä Luerlock-to-luerlock-välikappaleeseen tyhjä ruisku."; }
    private const string HINT = "Kiinnitä Luerlock-to-luerlock-välikappaleeseen 1ml ruisku.";
    #endregion

    #region Fields
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.MedicineToSyringe, TaskType.LuerlockAttach };
    private HashSet<Syringe> usedSyringes;
    private Dictionary<int, int> attachedSyringes = new Dictionary<int, int>();
    private CabinetBase laminarCabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for SyringeAttach task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public SyringeAttach() : base(TaskType.SyringeAttach, true, true) {
        Subscribe();
        usedSyringes = new HashSet<Syringe>();
        Points = 3;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        SubscribeEvent(AddSyringe, EventType.SyringeToLuerlock);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void AddSyringe(CallbackData data) {
        //virhetilanteet: pieni ruisku yhdistetty ennen lääkkeellisen ruiskun laittamista
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();

        if (s.Container.Capacity == RIGHT_SMALL_SYRINGE_CAPACITY) {
            usedSyringes.Add(s);
            Logger.Print("Added new syringe to used: " + usedSyringes.Count.ToString());
        }

        if (!attachedSyringes.ContainsKey(s.GetInstanceID()) && !s.hasBeenInBottle) {
            attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
        if (!IsPreviousTasksCompleted(requiredTasks)) {
            return;
        } else if (!laminarCabinet.GetContainedItems().Contains(s)) {
            CreateTaskMistake("Ruisku kiinnitettiin laminaarikaapin ulkopuolella", 1);
            attachedSyringes.Remove(s.GetInstanceID());
        } else {
            base.package.MoveTaskToManager(this);
        }
    }
    #endregion

    #region Public Methods

    public override void CompleteTask() {
        base.CompleteTask();
    }

    public override void FinishTask() {
        base.FinishTask();
        if (usedSyringes.Count < 6) {
            int minus = (int)Mathf.Round(Math.Abs(usedSyringes.Count - 6));
            CreateTaskMistake("Yksi tai useampi ruiskuista ei ollut oikean kokoinen", minus);
        }
    }

    public override string GetHint() {
        return HINT;
    }

    protected override void OnTaskComplete() {

    }
    #endregion
}