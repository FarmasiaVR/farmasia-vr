using System;
using System.Collections.Generic;
using UnityEngine;
public class SyringeAttach : TaskBase {

    #region Constants
    private const int RIGHT_SMALL_SYRINGE_CAPACITY = 1000;

    private const string DESCRIPTION = "Yhdistä Luerlock-to-luerlock-välikappaleeseen tyhjä ruisku.";
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
        points = 6;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        SubscribeEvent(AddSyringe, EventType.SyringeToLuerlock);
        SubscribeEvent(RemoveSyringe, EventType.SyringeFromLuerlock);
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
        }

        if (!attachedSyringes.ContainsKey(s.GetInstanceID()) && !s.hasBeenInBottle) {
            attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
        if (!IsPreviousTasksCompleted(requiredTasks)) {
            return;
        } else if (!laminarCabinet.GetContainedItems().Contains(s)) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.SyringeAttach);
            Popup("Ruisku kiinnitettiin laminaarikaapin ulkopuolella.", MsgType.Mistake, -1);
            attachedSyringes.Remove(s.GetInstanceID());
        } else {
            base.package.MoveTaskToManager(this);
        }
    }

    private void RemoveSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();


        //if (attachedSyringes.ContainsKey(s.GetInstanceID())) {
        //    if (IsPreviousTasksCompleted(requiredTasks)) {
        //        if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
        //            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.SyringeAttach);
        //            Popup("Ruisku poistettiin laminaarikaapin ulkopuolella.", MsgType.Mistake, -1);
        //            attachedSyringes.Remove(s.GetInstanceID());
        //        } else if (attachedSyringes[s.GetInstanceID()] != s.Container.Amount && attachedSyringes.Count == 6) {
        //            attachedSyringes[s.GetInstanceID()] = s.Container.Amount;
        //            FinishTask();
        //        }
        //    } else {
        //        attachedSyringes.Remove(s.GetInstanceID());
        //    }  
        //}
    }
    #endregion

    #region Public Methods

    public override void CompleteTask() {
        base.CompleteTask();
    }

    public override void FinishTask() {
        base.FinishTask();
        if (usedSyringes.Count >= 6) {
            Popup("Valitut ruiskut olivat oikean kokoisia.", MsgType.Notify);
        } else {
            Popup("Yksi tai useampi ruiskuista ei ollut oikean kokoinen.", MsgType.Mistake);
            G.Instance.Progress.Calculator.AddMistake("Väärän kokoinen ruisku luerlockiin");
            G.Instance.Progress.Calculator.SubtractWithScore(taskType, Math.Abs(usedSyringes.Count - 6));
        }
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }

    protected override void OnTaskComplete() {
        
    }
    #endregion
}