using System;

public class WearHeadCoverAndFaceMask : Task {

    public enum Conditions { WearingHeadCoverAndFaceMask };
    private bool headCover;
    private bool faceMask;

    public WearHeadCoverAndFaceMask() : base(TaskType.WearHeadCoverAndFaceMask, true) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackEquippedClothing, EventType.ProtectiveClothingEquipped);
    }

    private void TrackEquippedClothing(CallbackData data) {
        var clothing = (data.DataObject as ProtectiveClothing);
        if (clothing == null) return;

        if (clothing.type == "Suojapäähine") headCover = true;
        if (clothing.type == "Kasvomaski") faceMask = true;

        if (headCover && faceMask) {
            EnableCondition(Conditions.WearingHeadCoverAndFaceMask);
            CompleteTask();
        }
    }
}
