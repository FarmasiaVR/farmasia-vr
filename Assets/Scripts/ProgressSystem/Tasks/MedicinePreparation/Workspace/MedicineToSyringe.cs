using System;
using FarmasiaVR.Legacy;

public class MedicineToSyringe : Task {

    public enum Conditions { EnoughMedicineTaken }
    private const int CORRECT_SYRINGE_CAPACITY = 20000;
    private const int REQUIRED_MINIMUM_AMOUNT = 900;

    public MedicineToSyringe() : base(TaskType.MedicineToSyringe, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(DetachedNeedleFromSyringe, EventType.DetachedNeedleFromSyringe);
    }

    private void DetachedNeedleFromSyringe(CallbackData data) {
        Syringe syringe = (Syringe)data.DataObject;
        CheckMistakes(syringe);
        if (syringe.Container.Amount >= REQUIRED_MINIMUM_AMOUNT) {
            EnableCondition(Conditions.EnoughMedicineTaken);
            CompleteTask();
        }
    }

    private void CheckMistakes(Syringe syringe) {
        if (syringe.Container.Capacity != CORRECT_SYRINGE_CAPACITY) {
            CreateTaskMistake("Väärän kokoinen ruisku.", 1);
        }
        if (syringe.Container.Amount < REQUIRED_MINIMUM_AMOUNT) {
            CreateTaskMistake("Ruiskussa oli liian vähän lääkettä.", 1);
        }
    }
}
