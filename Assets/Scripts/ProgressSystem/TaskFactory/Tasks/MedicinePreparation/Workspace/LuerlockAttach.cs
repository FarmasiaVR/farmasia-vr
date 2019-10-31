using System;
using System.Collections.Generic;
using UnityEngine;
public class LuerlockAttach : TaskBase {
    #region Fields
    public enum Conditions { BigSyringeAttachedFirst }
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe};
    private CabinetBase laminarCabinet;
    private string description = "Kiinnitä luerlock-to-luerlock-välikappale.";
    private string hint = "Kiinnitä luerlock-to-luerlock-välikappale oikein 20ml ruiskuun.";
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for LuerlockAttach task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public LuerlockAttach() : base(TaskType.LuerlockAttach, true, true) {
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 1;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        base.SubscribeEvent(AttachLuerlock, EventType.AttachLuerlock);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }
    }

    /// <summary>
    /// Once fired by an event, checks if and how Luerlock was attached as well as previous required task completion.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void AttachLuerlock(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        if (laminarCabinet != null && !laminarCabinet.objectsInsideArea.Contains(g)) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.LuerlockAttach);
            UISystem.Instance.CreatePopup(-1, "Ruisku kiinnitettiin laminaarikaapin ulkopuolella.", MessageType.Mistake);
            return;
        }
        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            UISystem.Instance.CreatePopup("Ota ruiskuun lääkettä ennen luerlockiin yhdistämistä.", MessageType.Notify);
            return;
        }

        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            Syringe syringe = item.GetComponent<Syringe>();
            if (syringe.Container.Capacity == 5000 && syringe.Container.Amount > 0) {
                EnableCondition(Conditions.BigSyringeAttachedFirst);
            }
        }

        bool check = CheckClearConditions(true);
        if (!check) {
            UISystem.Instance.CreatePopup(0, "Luerlockia ei kiinnitetty ensin suureen ruiskuun.", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.LuerlockAttach);
            base.FinishTask();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Luerlockin kiinnittäminen onnistui.", MessageType.Notify);
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