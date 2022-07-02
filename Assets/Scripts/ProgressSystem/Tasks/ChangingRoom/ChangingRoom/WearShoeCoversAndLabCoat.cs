using System;

public class WearShoeCoversAndLabCoat : Task {

    public enum Conditions { WearingShoeCoversAndLabCoat };
    private bool shoeCovers;
    private bool labCoat;

    public WearShoeCoversAndLabCoat() : base(TaskType.WearShoeCoversAndLabCoat, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackEquippedClothing, EventType.ProtectiveClothingEquipped);
    }

    private void TrackEquippedClothing(CallbackData data) {
        var clothing = (data.DataObject as ProtectiveClothing);
        if (clothing == null) return;

        if (clothing.type == "Kengänsuojat") shoeCovers = true;
        if (clothing.type == "Laboratoriotakki") labCoat = true;

        if (shoeCovers && labCoat) {
            EnableCondition(Conditions.WearingShoeCoversAndLabCoat);
            CompleteTask();
        }
    }
}
