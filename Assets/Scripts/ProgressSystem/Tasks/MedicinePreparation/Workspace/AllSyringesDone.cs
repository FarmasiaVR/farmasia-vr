using System.Collections.Generic;
using System;
using UnityEngine;
using FarmasiaVR.Legacy;

public class AllSyringesDone : Task {

    public enum Conditions { CorrectAmountOfMedicineTransferredToAllSmallSyringes }
    private List<SmallSyringe> doneSmallSyringes;
    private const int REQUIRED_MINIMUM_AMOUNT = 150;

    public AllSyringesDone() : base(TaskType.AllSyringesDone, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        doneSmallSyringes = new List<SmallSyringe>();
    }

    public override void Subscribe() {
        base.SubscribeEvent(DetachSmallSyringe, EventType.SyringeFromLuerlock);
        base.SubscribeEvent(PushedFromBigSyringe, EventType.PushingToSmallerSyringe);
    }

    private void DetachSmallSyringe(CallbackData data) {
        GameObject obj = data.DataObject as GameObject;
        GeneralItem item = obj.GetComponent<GeneralItem>();
        SmallSyringe smallSyringe = item.GetComponent<SmallSyringe>();
        CheckMistakes(smallSyringe);
        if (smallSyringe.Container.Amount >= REQUIRED_MINIMUM_AMOUNT && !doneSmallSyringes.Contains(smallSyringe)) {
            doneSmallSyringes.Add(smallSyringe);
        }
        if (doneSmallSyringes.Count == 6) {
            EnableCondition(Conditions.CorrectAmountOfMedicineTransferredToAllSmallSyringes);
            CompleteTask();
        }
    }

    private void CheckMistakes(SmallSyringe smallSyringe) {
        if (smallSyringe.Container.Amount == REQUIRED_MINIMUM_AMOUNT) {
            return;
        }
        if (smallSyringe.Container.Amount > REQUIRED_MINIMUM_AMOUNT) {
            CreateTaskMistake("Ruiskussa oli liikaa lääkettä.", 1);
        } else if (smallSyringe.Container.Amount < REQUIRED_MINIMUM_AMOUNT) {
            CreateTaskMistake("Ruiskussa oli liian vähän lääkettä.", 1);
        }
    }

    private void PushedFromBigSyringe(CallbackData data) {
        Popup("Älä työnnä isosta ruiskusta pieneen. Vedä pienellä.", MsgType.Notify);
    }
}
