using System;
using System.Collections.Generic;
using FarmasiaVR.Legacy;

public class WearSleeveCoversAndProtectiveGloves : Task {

    public enum Conditions { WearingSleeveCoversAndProtectiveGloves };
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.WearHeadCoverAndFaceMask, TaskType.WashHandsInPreperationRoom };
    private bool sleeveCovers;
    private bool protectiveGloves;

    public WearSleeveCoversAndProtectiveGloves() : base(TaskType.WearSleeveCoversAndProtectiveGloves, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackEquippedClothing, EventType.ProtectiveClothingEquipped);
    }

    private void TrackEquippedClothing(CallbackData data) {
        var clothing = (data.DataObject as ProtectiveClothing);
        if (!IsPreviousTasksCompleted(requiredTasks)) {
            if (clothing.type == ClothingType.SleeveCovers || clothing.type == ClothingType.ProtectiveGloves) {
                Translater.Translate("DressingRoom", "MistakeHandsWashedAgain", (translatedText) => { CreateTaskMistake(translatedText, 1); });
                return;
            }
        }
        if (!sleeveCovers && !protectiveGloves && clothing.type == ClothingType.ProtectiveGloves) 
            Translater.Translate("DressingRoom", "MistakeSleevesBeforeGloves", (translatedText) => { CreateTaskMistake("Hihansuojat tulee laittaa päälle ennen suojakäsineitä", 1); });
        if (clothing.type == ClothingType.SleeveCovers) sleeveCovers = true;
        if (clothing.type == ClothingType.ProtectiveGloves) protectiveGloves = true;
        if (sleeveCovers && protectiveGloves) {
            EnableCondition(Conditions.WearingSleeveCoversAndProtectiveGloves);
            CompleteTask();
        }
    }
}
