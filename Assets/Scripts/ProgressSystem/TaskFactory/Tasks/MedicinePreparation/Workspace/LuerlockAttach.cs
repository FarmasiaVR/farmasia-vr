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
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.MedicineToSyringe };
    private CabinetBase laminarCabinet;

    private bool firstCheckDone = false;
    #endregion

    #region Constructor
    public LuerlockAttach() : base(TaskType.LuerlockAttach, true, true) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 1;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(AttachLuerlock, EventType.AttachLuerlock);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
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
            G.Instance.Audio.Play(AudioClipType.MistakeMessage);
            return;
        } else if (!laminarCabinet.objectsInsideArea.Contains(g)) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.LuerlockAttach);
            UISystem.Instance.CreatePopup(-1, "Ruisku kiinnitettiin laminaarikaapin ulkopuolella.", MsgType.Mistake);
            G.Instance.Audio.Play(AudioClipType.MistakeMessage);
            return;
        }

        if (!IsPreviousTasksCompleted(requiredTasks)) {
            Popup("Ota ruiskuun lääkettä ennen luerlockiin yhdistämistä.", MsgType.Notify);
            return;
        }

        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            MedicineSyringeCheck(item);
        }

        CompleteTask();

        if (!IsCompleted()) {
            if (!firstCheckDone) {
                Popup("Luerlockia ei kiinnitetty ensin lääkkeelliseen ruiskuun.", MsgType.Mistake, -1);
                G.Instance.Progress.Calculator.Subtract(TaskType.LuerlockAttach);
                firstCheckDone = true;
            } else {
                Popup("Kiinnitä ensin lääkkeellinen ruisku", MsgType.Mistake);
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

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }

    protected override void OnTaskComplete() {
        Popup("Luerlockin kiinnittäminen onnistui.", MsgType.Notify, 1);
    }
    #endregion
}