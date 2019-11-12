using System;
using System.Collections.Generic;
using UnityEngine;
public class SyringeAttach : TaskBase {
    #region Fields
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe, TaskType.LuerlockAttach};
    private Dictionary<int, int> attachedSyringes = new Dictionary<int, int>();
    private CabinetBase laminarCabinet;
    private string description = "Yhdistä Luerlock-to-luerlock-välikappaleeseen tyhjä ruisku.";
    private string hint = "Kiinnitä Luerlock-to-luerlock-välikappaleeseen 1ml ruisku.";

    private const int RightSmallSyringeCapacity = 1000;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for SyringeAttach task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public SyringeAttach() : base(TaskType.SyringeAttach, true, true) {
        Subscribe();
        points = 6;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        SubscribeEvent(AddSyringe, EventType.SyringeToLuerlock);
        SubscribeEvent(RemoveSyringe, EventType.SyringeFromLuerlock);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }        
    }

    private void AddSyringe(CallbackData data) {
        //virhetilanteet: pieni ruisku yhdistetty ennen lääkkeellisen ruiskun laittamista
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();
        if (!attachedSyringes.ContainsKey(s.GetInstanceID()) && !s.hasBeenInBottle) {
            attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            return;
        } else if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.SyringeAttach);
            UISystem.Instance.CreatePopup(-1, "Ruisku kiinnitettiin laminaarikaapin ulkopuolella.", MsgType.Mistake);
            attachedSyringes.Remove(s.GetInstanceID());
        } else {
            base.package.MoveTaskToManager(this);
        }
    }

    private void RemoveSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();

        if (attachedSyringes.ContainsKey(s.GetInstanceID())) {
            if (CheckPreviousTaskCompletion(requiredTasks)) {
                if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
                    G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.SyringeAttach);
                    UISystem.Instance.CreatePopup(-1, "Ruisku poistettiin laminaarikaapin ulkopuolella.", MsgType.Mistake);
                    attachedSyringes.Remove(s.GetInstanceID());
                } else if (attachedSyringes[s.GetInstanceID()] != s.Container.Amount && attachedSyringes.Count == 6) {
                    FinishTask();
                }
            } else {
                attachedSyringes.Remove(s.GetInstanceID());
            }  
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        int rightSize = 0;
        foreach (GameObject value in laminarCabinet.objectsInsideArea) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            if (type == ObjectType.Syringe) {
                Syringe s = item.GetComponent<Syringe>();
                if (s.Container.Capacity == RightSmallSyringeCapacity && attachedSyringes.ContainsKey(s.GetInstanceID())) {
                    rightSize++;
                }
            }   
        }
        if (rightSize == 6) {
            UISystem.Instance.CreatePopup("Valitut ruiskut olivat oikean kokoisia.", MsgType.Notify);
        } else {
            UISystem.Instance.CreatePopup("Yksi tai useampi ruiskuista ei ollut oikean kokoinen.", MsgType.Notify);
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