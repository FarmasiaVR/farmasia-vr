using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of medicine pulled into smaller syringes through LuerLock.
/// </summary>
public class CorrectAmountOfMedicineSelected : TaskBase {

    #region Constants
    private const int MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 150;
    private const int MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 150;

    private const string DESCRIPTION = "Vedä ruiskuun lääkettä.";
    private const string HINT = "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.";
    #endregion

    #region Fields
    public enum Conditions { }
    private Dictionary<int, int> attachedSyringes = new Dictionary<int, int>();
    private Dictionary<Syringe, int> usedSyringes;
    #endregion

    #region Constructor
    public CorrectAmountOfMedicineSelected() : base(TaskType.CorrectAmountOfMedicineSelected, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        usedSyringes = new Dictionary<Syringe, int>();
        points = 6;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(AddSyringe, EventType.SyringeToLuerlock);
        base.SubscribeEvent(RemoveSyringe, EventType.SyringeFromLuerlock);
        base.SubscribeEvent(InvalidSyringePush, EventType.PushingToSmallerSyringe);
    }

    private void AddSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();

        if (!usedSyringes.ContainsKey(s)) {
            usedSyringes.Add(s, 0);
        }

        if (!attachedSyringes.ContainsKey(s.GetInstanceID()) && !s.hasBeenInBottle) {
            attachedSyringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
    }

    private void RemoveSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        Syringe s = item.GetComponent<Syringe>();

        int minus = 0;
        int oldMinus = 0;

        if (!usedSyringes.ContainsKey(s)) {
            return;
        }
        oldMinus = usedSyringes[s];


        if (s.Container.Amount != MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && !s.IsClean) {
            minus++;
            minus++;
            Popup("Väärä määrä lääkettä ruiskussa ja likainen", MsgType.Mistake, -2);
        } else if (s.Container.Amount != MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
            minus++;
            Popup("Väärä määrä lääkettä", MsgType.Mistake, -1);
        } else if (!s.IsClean) {
            minus++;
            Popup("Ruisku tai luerlock oli likainen", MsgType.Mistake, -1);
        } else {
            Popup("Ruiskuun otettiin oikea määrä lääkettä.", MsgType.Done);
        }

        if (minus > oldMinus) {
            usedSyringes[s] = minus;
        }

        if (usedSyringes.Count >= 7) {
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectAmountOfMedicineSelected, GetTotalMinus());
            G.Instance.Progress.ForceCloseTask(TaskType.SyringeAttach, false);
            G.Instance.Progress.ForceCloseTask(taskType, false);
            Logger.Print("CLOSED SYRINGE ATTACH AND CORRECT AMOUNT");
        }
    }

    private int GetTotalMinus() {
        int sum = 0;
        foreach (var pair in usedSyringes) {
            sum += pair.Value;
        }
        return sum;
    }

    private void InvalidSyringePush(CallbackData data) {
        G.Instance.Progress.Calculator.Subtract(TaskType.CorrectAmountOfMedicineSelected);
        Popup("Älä työnnä isosta ruiskusta pieneen. Vedä pienellä.", MsgType.Mistake, -1);
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
        int rightAmount = 0;
        foreach (var amount in attachedSyringes.Values) {
            if (amount >= MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && amount <= MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
                rightAmount++;
            }
        }
        if (rightAmount == 6) {
            Popup("Valittiin oikea määrä lääkettä.", MsgType.Notify);
        } else {
            Popup("Yhdessä tai useammassa ruiskussa oli väärä määrä lääkettä.", MsgType.Notify);
        }
    }
    #endregion
}