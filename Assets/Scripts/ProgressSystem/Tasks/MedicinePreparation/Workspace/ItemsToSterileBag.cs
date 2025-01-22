using System;
using FarmasiaVR.Legacy;

public class ItemsToSterileBag : Task {

    public enum Conditions { AllSmallSyringesInsideSterileBag }
    private const int REQUIRED_MINIMUM_AMOUNT = 150;

    public ItemsToSterileBag() : base(TaskType.ItemsToSterileBag, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(OnSterileBagClose, EventType.CloseSterileBag);
    }

    private void OnSterileBagClose(CallbackData data) {
        // All tasks that haven't been completed before closing the sterile bag will be failed
        G.Instance.Progress.ForceCloseActiveTasksInPackage(this, G.Instance.Progress.CurrentPackage);
        SterileBag sterileBag = (SterileBag)data.DataObject;
        CheckMistakes(sterileBag);
        EnableCondition(Conditions.AllSmallSyringesInsideSterileBag);
        CompleteTask();
    }

    private void CheckMistakes(SterileBag sterileBag) {
        int missingSyringeCaps = 0;
        int incorrectAmountOfMedicine = 0;
        foreach (SmallSyringe syringe in sterileBag.syringes) {
            if (!syringe.HasSyringeCap) {
                missingSyringeCaps++;
            }
            if (syringe.Container.Amount != REQUIRED_MINIMUM_AMOUNT) {
                incorrectAmountOfMedicine++;
            }
        }
        if (missingSyringeCaps != 0) {
            CreateTaskMistake("Yhdeltä tai useammalta ruiskulta puuttui korkki.", missingSyringeCaps);
        }
        if (incorrectAmountOfMedicine != 0) {
            CreateTaskMistake("Yhdessä tai useammassa ruiskussa ei ollut oikea määrä lääkettä.", incorrectAmountOfMedicine);
        }
    }
}
