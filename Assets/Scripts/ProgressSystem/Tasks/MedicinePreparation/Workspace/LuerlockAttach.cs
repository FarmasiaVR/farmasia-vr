using System;
using System.Collections.Generic;
using UnityEngine;
public class LuerlockAttach : Task {

    #region Constants
    public override string Description { get => "Kiinnitä lääkkeellinen ruisku luerlock-to-luerlock-välikappaleeseen."; }
    private const string HINT = "Kiinnitä luerlock-to-luerlock-välikappale oikein 20ml ruiskuun.";
    #endregion

    #region Fields
    public enum Conditions { SyringeWithMedicineAttached }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.MedicineToSyringe };
    private CabinetBase laminarCabinet;
    private bool fail = false;
    private bool firstCheckDone = false;
    #endregion

    #region Constructor
    public LuerlockAttach() : base(TaskType.LuerlockAttach, true, true) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        Points = 1;
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
            CreateTaskMistake("Ruisku kiinnitettiin liian aikaisin.", 1);
            Fail();
            return;
        } else if (!laminarCabinet.GetContainedItems().Contains(item)) {
            CreateTaskMistake("Ruisku kiinnitettiin laminaarikaapin ulkopuolella", 1);
            Fail();
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

        if (!item.IsClean) {
            CreateTaskMistake("Ruisku oli likainen", 1);
            Fail();
        }

        CompleteTask();

        if (!Completed) {
            if (!firstCheckDone) {
                CreateTaskMistake("Luerlockia ei kiinnitetty ensin lääkkeelliseen ruiskuun", 1);
                Popup("Luerlockia ei kiinnitetty ensin lääkkeelliseen ruiskuun.", MsgType.Mistake, -1);
                firstCheckDone = true;
                Fail();
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

    private void Fail() {
        fail = true;
    }
    #endregion

    #region Public Methods

    public override string Hint {
        get => HINT;
    }

    protected override void OnTaskComplete() {
        if (!fail) {
            Popup("Luerlockin kiinnittäminen onnistui.", MsgType.Done);
        }
    }
    #endregion
}