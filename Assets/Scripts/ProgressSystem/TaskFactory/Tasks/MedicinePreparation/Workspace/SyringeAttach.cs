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
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe, TaskType.LuerlockAttach};
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
        points = 6;
    }
    #endregion

    #region Event Subscriptions
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
            G.Instance.Audio.Play(AudioClipType.MistakeMessage);
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
                    G.Instance.Audio.Play(AudioClipType.MistakeMessage);
                    attachedSyringes.Remove(s.GetInstanceID());
                } else if (attachedSyringes[s.GetInstanceID()] != s.Container.Amount && attachedSyringes.Count == 6) {
                    attachedSyringes[s.GetInstanceID()] = s.Container.Amount;
                    FinishTask();
                }
            } else {
                attachedSyringes.Remove(s.GetInstanceID());
            }  
        }
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        int rightSize = 0;
        foreach (GameObject value in laminarCabinet.objectsInsideArea) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            if (type == ObjectType.Syringe) {
                Syringe s = item.GetComponent<Syringe>();
                if (s.Container.Capacity == RIGHT_SMALL_SYRINGE_CAPACITY && attachedSyringes.ContainsKey(s.GetInstanceID())) {
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

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }
    #endregion
}