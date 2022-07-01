using System;

public class PutOnProtectiveClothing : Task {

    public enum Conditions { AllClothesOn }
    private bool headCover;
    private bool faceMask;
    private bool labCoat;
    private bool sleeveCovers;
    private bool sterileGloves;
    private bool shoeCovers;

    public PutOnProtectiveClothing() : base(TaskType.PutOnProtectiveClothing, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackEquippedItems, EventType.ProtectiveClothingEquipped);
    }

    private void TrackEquippedItems(CallbackData data) {
        var clothing = (data.DataObject as ProtectiveClothing);
        if (clothing == null) return;

        if (clothing.type == "Suojapäähine") headCover = true;
        if (clothing.type == "Kasvomaski") faceMask = true;
        if (clothing.type == "Laboratoriotakki") labCoat = true;
        if (clothing.type == "Hihasuojat") sleeveCovers = true;
        if (clothing.type == "Suojakäsineet") sterileGloves = true;
        if (clothing.type == "Kengänsuojat") shoeCovers = true;

        if (headCover && faceMask && labCoat && sleeveCovers && sterileGloves && shoeCovers) {
            Logger.Print("Task completed!");
            CompleteTask();
        }
    }
}
