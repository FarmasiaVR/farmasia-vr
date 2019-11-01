using System;
using System.Collections.Generic;
using UnityEngine;
public class SyringeAttach : TaskBase {
    #region Fields
    public enum Conditions { SmallSyringesAttached }
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe, TaskType.LuerlockAttach};
    private Dictionary<int, int> attachedSyringes = new Dictionary<int, int>();
    private int syringes;
    private int smallSyringes;
    private CabinetBase laminarCabinet;
    private string description = "Yhdistä Luerlock-to-luerlock-välikappaleeseen toinen ruisku.";
    private string hint = "Kiinnitä Luerlock-to-luerlock-välikappaleeseen myös 1.0ml ruisku.";
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for SyringeAttach task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public SyringeAttach() : base(TaskType.SyringeAttach, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        syringes = 0;
        smallSyringes = 0;
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
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();
        attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);

        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            return;
        } else if (!laminarCabinet.objectsInsideArea.Contains(s.gameObject)) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.SyringeAttach);
            UISystem.Instance.CreatePopup(-1, "Ruisku kiinnitettiin laminaarikaapin ulkopuolella.", MessageType.Mistake);
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
                    UISystem.Instance.CreatePopup(-1, "Ruisku poistettiin laminaarikaapin ulkopuolella.", MessageType.Mistake);
                } else if (attachedSyringes[s.GetInstanceID()] != s.Container.Amount) {
                    AttachSyringe(s);
                }
            }  
            attachedSyringes.Remove(s.GetInstanceID());
        }
    }

    /// <summary>
    /// Once fired by an event, checks if syringe was attached to Luerlock, which syringe size was chosen
    /// as well as previous required task completion.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void AttachSyringe(Syringe syringe) {
        syringes++;
        if (syringe.Container.Capacity == 1000) {
            smallSyringes++;
        }

        if (smallSyringes == 6) {
            EnableCondition(Conditions.SmallSyringesAttached);
        }
        if (syringes == 6) {
            bool check = CheckClearConditions(true);
            if (!check) {
                UISystem.Instance.CreatePopup(smallSyringes, "Yhden tai useamman ruiskun koko oli väärä.", MessageType.Mistake);
                G.Instance.Progress.Calculator.SubtractWithScore(TaskType.SyringeAttach, syringes - smallSyringes);
                base.FinishTask();
            }
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(6, "Valitut ruiskut olivat oikean kokoisia", MessageType.Notify);
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