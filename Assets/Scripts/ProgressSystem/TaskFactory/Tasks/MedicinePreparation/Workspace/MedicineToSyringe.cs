using System;
using System.Collections.Generic;
using UnityEngine;
public class MedicineToSyringe : TaskBase {

    #region Constants
    private const int RIGHT_SYRINGE_CAPACITY = 20000;
    private const int MINIMUM_AMOUNT_OF_MEDICINE_IN_BIG_SYRINGE = 900;

    private const string DESCRIPTION = "Valmistele välineet ja ota ruiskulla ja neulalla lääkettä lääkeainepullosta.";
    private const string HINT = "Valitse oikeankokoinen ruisku (20ml), jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula.";

    private Syringe syringe;
    #endregion

    #region Fields
    // private Dictionary<int, int> syringes = new Dictionary<int, int>();
    public enum Conditions { }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.CorrectItemsInLaminarCabinet };
    private CabinetBase laminarCabinet;
    #endregion

    #region States
    private bool takenBeforeTime = false;
    #endregion

    #region Constructor
    public MedicineToSyringe() : base(TaskType.MedicineToSyringe, true, true) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        SubscribeEvent(NeedleWithSyringeInsertedIntoBottle, EventType.SyringeWithNeedleEntersBottle);
        SubscribeEvent(FinishedTakingMedicineToSyringe, EventType.FinishedTakingMedicineToSyringe);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void NeedleWithSyringeInsertedIntoBottle(CallbackData data) {
        Syringe s = data.DataObject as Syringe;
        if (!IsPreviousTasksCompleted(requiredTasks) && G.Instance.Progress.CurrentPackage.name == PackageName.Workspace) {
            Popup("Siirrä kaikki tarvittavat työvälineet ensin laminaarikaappiin.", MsgType.Notify);
            G.Instance.Progress.ForceCloseTask(TaskType.CorrectItemsInLaminarCabinet, false);
            G.Instance.Progress.ForceCloseTask(TaskType.DisinfectBottles, false);
        }
    }

    private void FinishedTakingMedicineToSyringe(CallbackData data) {
        syringe = (Syringe)data.DataObject;

        if (laminarCabinet == null) {
            if (!takenBeforeTime) {
                G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
                Popup("Lääkettä yritettiin ottaa liian aikaisin.", MsgType.Mistake, -1);
                takenBeforeTime = true;
            } else {
                Popup("Lääkettä yritettiin ottaa liian aikaisin.", MsgType.Mistake);
            }
        } else if (!laminarCabinet.GetContainedItems().Contains(syringe)) {
            G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.MedicineToSyringe);
            Popup("Lääkettä yritettiin ottaa laminaarikaapin ulkopuolella.", MsgType.Mistake, -1);
        } else {
            if (!IsPreviousTasksCompleted(requiredTasks)) {
                Popup("Tarvittavia työvälineitä ei siirretty laminaarikaappiin.", MsgType.Mistake, -1);
                G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.CorrectItemsInLaminarCabinet);
            }
            //CheckConditions(s);
        }
        CompleteTask();
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
        (G.Instance.Scene as MedicinePreparationScene).NeedleUsed = true;

        if (syringe == null) {
            return;
        }

        bool fail = false;
        if (syringe.Container.Capacity != RIGHT_SYRINGE_CAPACITY) {
            Popup("Väärän kokoinen ruisku", MsgType.Mistake, -1);
            G.Instance.Progress.Calculator.Subtract(taskType);
            fail = true;
        }
        if (syringe.Container.Amount < MINIMUM_AMOUNT_OF_MEDICINE_IN_BIG_SYRINGE) {
            Popup("Liian vähän lääkettä", MsgType.Mistake, -1);
            G.Instance.Progress.Calculator.Subtract(taskType);
            fail = true;
        }
        if (!syringe.IsClean) {
            G.Instance.Progress.Calculator.Subtract(taskType);
            Popup("Ruisku tai pullo oli likainen", MsgType.Mistake, -1);
            fail = true;
        }

        if (!fail) {
            Popup("Lääkkeen ottaminen onnistui.", MsgType.Done, 2);
        }
    }
    #endregion
}