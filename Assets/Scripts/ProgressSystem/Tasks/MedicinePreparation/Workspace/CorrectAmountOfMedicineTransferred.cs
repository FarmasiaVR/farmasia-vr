using System;
using UnityEngine;

public class CorrectAmountOfMedicineTransferred : Task {

    public enum Conditions { MedicineTransferred }
    private const int REQUIRED_MINIMUM_AMOUNT = 150;

    public CorrectAmountOfMedicineTransferred() : base(TaskType.CorrectAmountOfMedicineTransferred, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(DetachSmallSyringe, EventType.SyringeFromLuerlock);
    }

    private void DetachSmallSyringe(CallbackData data) {
        GameObject obj = data.DataObject as GameObject;
        GeneralItem item = obj.GetComponent<GeneralItem>();
        SmallSyringe smallSyringe = item.GetComponent<SmallSyringe>();
        // Checking for mistakes is done in AllSyringesDone task. This tasks purpose is to just teach the player what to do
        if (smallSyringe.Container.Amount >= REQUIRED_MINIMUM_AMOUNT) {
            EnableCondition(Conditions.MedicineTransferred);
            CompleteTask();
        }
    }
}
