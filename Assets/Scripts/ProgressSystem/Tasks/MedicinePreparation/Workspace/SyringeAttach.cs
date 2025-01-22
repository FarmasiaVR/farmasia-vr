using System;
using UnityEngine;
using FarmasiaVR.Legacy;

public class SyringeAttach : Task {

    public enum Conditions { SmallSyringeAttachedToLuerlock }
    private const int CORRECT_SYRINGE_CAPACITY = 1000;

    public SyringeAttach() : base(TaskType.SyringeAttach, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(SyringeToLuerlock, EventType.SyringeToLuerlock);
    }

    private void SyringeToLuerlock(CallbackData data) {
        GameObject obj = data.DataObject as GameObject;
        GeneralItem item = obj.GetComponent<GeneralItem>();
        SmallSyringe smallSyringe = item.GetComponent<SmallSyringe>();
        if (smallSyringe.Container.Capacity == CORRECT_SYRINGE_CAPACITY) {
            EnableCondition(Conditions.SmallSyringeAttachedToLuerlock);
            CompleteTask();
        }
    }
}
