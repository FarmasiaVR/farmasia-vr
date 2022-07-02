using System;

public class WearSleeveCoversAndProtectiveGloves : Task {

    public enum Conditions { WearingSleeveCoversAndProtectiveGloves };
    private bool sleeveCovers;
    private bool protectiveGloves;

    public WearSleeveCoversAndProtectiveGloves() : base(TaskType.WearSleeveCoversAndProtectiveGloves, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackEquippedClothing, EventType.ProtectiveClothingEquipped);
    }

    private void TrackEquippedClothing(CallbackData data) {
        var clothing = (data.DataObject as ProtectiveClothing);
        if (clothing == null) return;

        if (clothing.type == "Hihasuojat") sleeveCovers = true;
        if (clothing.type == "Suojakäsineet") protectiveGloves = true;

        if (sleeveCovers && protectiveGloves) {
            EnableCondition(Conditions.WearingSleeveCoversAndProtectiveGloves);
            CompleteTask();
        }
    }
}
