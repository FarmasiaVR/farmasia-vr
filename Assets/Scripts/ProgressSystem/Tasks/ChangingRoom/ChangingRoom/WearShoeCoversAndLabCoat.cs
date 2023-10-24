using System;
using FarmasiaVR.Legacy;

public class WearShoeCoversAndLabCoat : Task {

    public enum Conditions { WearingShoeCoversAndLabCoat };
    private bool taskCompleted;
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
        if (taskCompleted) return;
        var clothing = (data.DataObject as ProtectiveClothing);
        if (!shoeCovers && !labCoat && clothing.type == ClothingType.LabCoat) 
            Translater.Translate("DressingRoom", "MistakeShoeCoversBeforeLabCoat", (translatedText) => 
            { CreateTaskMistake(translatedText, 1); });
        if (clothing.type == ClothingType.ShoeCovers) shoeCovers = true;
        if (clothing.type == ClothingType.LabCoat) labCoat = true;
        if (shoeCovers && labCoat) {
            EnableCondition(Conditions.WearingShoeCoversAndLabCoat);
            CompleteTask();
            taskCompleted = true;
        }
    }
}
