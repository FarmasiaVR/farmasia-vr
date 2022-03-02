using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of medicine pulled into smaller SmallSyringes through LuerLock.
/// </summary>
public class CorrectAmountOfMedicineSelected : TaskBase {

    #region Constants
    private const int MINIMUM_CORRECT_AMOUNT_IN_SMALL_SmallSyringe = 150;
    private const int MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SmallSyringe = 150;

    private const string DESCRIPTION = "Vedä ruiskuun lääkettä.";
    private const string HINT = "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.";
    #endregion

    #region Fields
    public enum Conditions { }
    private Dictionary<int, int> attachedSmallSyringes = new Dictionary<int, int>();
    private Dictionary<SmallSyringe, int> usedSmallSyringes;
    #endregion

    #region Constructor
    public CorrectAmountOfMedicineSelected() : base(TaskType.CorrectAmountOfMedicineSelected, true, true) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        usedSmallSyringes = new Dictionary<SmallSyringe, int>();
        points = 6;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(AddSmallSyringe, EventType.SyringeToLuerlock);
        base.SubscribeEvent(RemoveSmallSyringe, EventType.SyringeFromLuerlock);
        base.SubscribeEvent(InvalidSmallSyringePush, EventType.PushingToSmallerSyringe);
    }

    private void AddSmallSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        SmallSyringe s = item.GetComponent<SmallSyringe>();

        if (s.Container.Capacity == 20000) {
            return;
        }

        if (!usedSmallSyringes.ContainsKey(s)) {
            usedSmallSyringes.Add(s, 0);
        }

        if (!attachedSmallSyringes.ContainsKey(s.GetInstanceID()) && !s.hasBeenInBottle) {
            attachedSmallSyringes.Add(s.GetInstanceID(), s.Container.Amount);
        }
    }

    private void RemoveSmallSyringe(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        SmallSyringe s = item.GetComponent<SmallSyringe>();

        int minus = 0;
        int oldMinus = 0;

        if (!usedSmallSyringes.ContainsKey(s)) {
            return;
        }
        oldMinus = usedSmallSyringes[s];

        if (s.Container.Amount != MINIMUM_CORRECT_AMOUNT_IN_SMALL_SmallSyringe && !s.IsClean) {
            minus += 2;
            CreateTaskMistake("Väärä määrä lääkettä ruiskussa ja likainen", 0);
        } else if (s.Container.Amount != MINIMUM_CORRECT_AMOUNT_IN_SMALL_SmallSyringe) {
            minus++;
            CreateTaskMistake("Väärä määrä lääkettä", 0);
        } else if (!s.IsClean) {
            minus++;
            CreateTaskMistake("Ruisku tai luerlock oli likainen", 0);
        } else {
            Popup("Ruiskuun otettiin oikea määrä lääkettä.", MsgType.Done);
        }

        if (minus > oldMinus) {
            usedSmallSyringes[s] = minus;
        }

        if (usedSmallSyringes.Count >= 6) {
            Logger.Print("CLOSED SmallSyringe ATTACH AND CORRECT AMOUNT");
            CreateTaskMistake(null, GetTotalMinus());
            G.Instance.Progress.ForceCloseTask(taskType, false);
            G.Instance.Progress.ForceCloseTask(TaskType.SyringeAttach, false);
        }
    }

    private int GetTotalMinus() {
        int sum = 0;
        foreach (var pair in usedSmallSyringes) {
            sum += pair.Value;
        }
        return sum;
    }

    private void InvalidSmallSyringePush(CallbackData data) {
        CreateTaskMistake("Älä työnnä isosta ruiskusta pieneen. Vedä pienellä.", 1);
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
    }
    #endregion
}