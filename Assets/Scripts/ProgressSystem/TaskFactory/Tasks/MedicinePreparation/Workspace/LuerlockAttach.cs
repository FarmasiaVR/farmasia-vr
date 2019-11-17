using System;
using System.Collections.Generic;
using UnityEngine;
public class LuerlockAttach : TaskBase {

    #region Constants
    private const string DESCRIPTION = "Kiinnitä lääkkeellinen ruisku luerlock-to-luerlock-välikappaleeseen.";
    private const string HINT = "Kiinnitä luerlock-to-luerlock-välikappale oikein 20ml ruiskuun.";
    #endregion

    #region Fields
    public enum Conditions { SyringeWithMedicineAttached }
    private List<TaskType> requiredTasks = new List<TaskType> {TaskType.MedicineToSyringe};
    private CabinetBase laminarCabinet;
    
    private int checkTimes;
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
        checkTimes = 0;
    }
    #endregion

    #region Event Subscriptions
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

        if (laminarCabinet == null) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.LuerlockAttach);
            UISystem.Instance.CreatePopup(-1, "Ruisku kiinnitettiin liian aikaisin.", MsgType.Mistake);
            return;
        } else if (!laminarCabinet.objectsInsideArea.Contains(g)) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.LuerlockAttach);
            UISystem.Instance.CreatePopup(-1, "Ruisku kiinnitettiin laminaarikaapin ulkopuolella.", MsgType.Mistake);
            return;
        }
        
        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            foreach (ITask task in G.Instance.Progress.CurrentPackage.activeTasks) {
                if (task.GetTaskType() == TaskType.CorrectItemsInLaminarCabinet) {
                    task.UnsubscribeAllEvents();
                    task.RemoveFromPackage();
                    UISystem.Instance.CreatePopup(-1, "Tarvittavia työvälineitä ei siirretty laminaarikaappiin.", MsgType.Mistake);
                    G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectItemsInLaminarCabinet);
                    break;
                }
            }
                         
            UISystem.Instance.CreatePopup("Ota ruiskuun lääkettä ennen luerlockiin yhdistämistä.", MsgType.Notify);
            return;
        }

        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            MedicineSyringeCheck(item);
        }

        checkTimes++;
        if (!CheckClearConditions(true)) {
            if (checkTimes == 1) {
                UISystem.Instance.CreatePopup(0, "Luerlockia ei kiinnitetty ensin lääkkeelliseen ruiskuun.", MsgType.Mistake);
                G.Instance.Progress.Calculator.Subtract(TaskType.LuerlockAttach);
            } else {
                UISystem.Instance.CreatePopup("Kiinnitä ensin lääkkeellinen ruisku", MsgType.Mistake);
            }  
        }
    }

    private void MedicineSyringeCheck(GeneralItem item) {
        Syringe syringe = item.GetComponent<Syringe>();
        if (syringe.Container.Amount > 0) {
            EnableCondition(Conditions.SyringeWithMedicineAttached);
        }
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (checkTimes == 1) {
            UISystem.Instance.CreatePopup(1, "Luerlockin kiinnittäminen onnistui.", MsgType.Notify);    
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