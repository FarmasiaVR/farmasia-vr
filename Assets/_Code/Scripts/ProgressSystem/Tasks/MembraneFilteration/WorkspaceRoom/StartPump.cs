using System;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

class StartPump : Task {

    public enum Conditions { FilterIsEmpty }

    private PumpFilter pumpFilter;
    
    public StartPump(TaskType type) : base(type, false) {

        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        
    }

    public override void Subscribe() {
        SubscribeEvent(OnFilterEmpty, EventType.FilterEmptied);
    }

    private void OnFilterEmpty(CallbackData data) {
        if (!Started) return;

        EnableCondition(Conditions.FilterIsEmpty);
        CompleteTask();        
    }

    private void CheckMistakes() {
        if (pumpFilter.Container.Amount > 0) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "FilterStillHasLiquid"), 1);
        }
    }
}