using System;

public class ItemsToSterileBag : Task {

    public enum Conditions { AllSmallSyringesInsideSterileBag }

    public ItemsToSterileBag() : base(TaskType.ItemsToSterileBag, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(OnSterileBagClose, EventType.CloseSterileBag);
    }

    private void OnSterileBagClose(CallbackData data) {
        SterileBag sterileBag = (SterileBag)data.DataObject;
        CheckMistakes(sterileBag);
        EnableCondition(Conditions.AllSmallSyringesInsideSterileBag);
        CompleteTask();
    }

    private void CheckMistakes(SterileBag sterileBag) {
        int missingSyringeCaps = 0;
        foreach (SmallSyringe syringe in sterileBag.Syringes) {
            if (!syringe.HasSyringeCap) {
                missingSyringeCaps++;
            }
        }
        if (missingSyringeCaps != 0) {
            CreateTaskMistake("Yhdeltä tai useammalta ruiskulta puuttui korkki.", missingSyringeCaps);
        }
    }
}
