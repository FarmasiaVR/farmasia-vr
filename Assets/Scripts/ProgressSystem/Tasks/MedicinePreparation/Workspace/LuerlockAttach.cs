using System;
using UnityEngine;

public class LuerlockAttach : Task {

    public enum Conditions { LuerlockAttachedToSyringeWithMedicine }
    private const int REQUIRED_MINIMUM_AMOUNT = 900;

    public LuerlockAttach() : base(TaskType.LuerlockAttach, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(AttachLuerlock, EventType.AttachLuerlock);
    }

    private void AttachLuerlock(CallbackData data) {
        GameObject obj = (GameObject)data.DataObject;
        GeneralItem item = obj.GetComponent<GeneralItem>();
        Syringe syringe = item.GetComponent<Syringe>(); ;
        CheckMistakes(syringe);
        if (syringe.Container.Amount >= REQUIRED_MINIMUM_AMOUNT) {
            EnableCondition(Conditions.LuerlockAttachedToSyringeWithMedicine);
            CompleteTask();
        }
    }

    private void CheckMistakes(Syringe syringe) {
        if (syringe.Container.Amount == 0) {
            CreateTaskMistake("Luerlockia ei kiinnitetty ruiskuun mikä sisälsi lääkettä.", 1);
        } else if (syringe.Container.Amount < REQUIRED_MINIMUM_AMOUNT) {
            CreateTaskMistake("Luerlock yhdistettiin ruiskuun mikä ei sisältänyt tarpeeksi lääkettä.", 1);
        }
    }
}
