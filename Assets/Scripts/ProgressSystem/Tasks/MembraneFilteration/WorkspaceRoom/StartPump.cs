using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StartPump: Task {

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
            CreateTaskMistake("Suodattimessa on vielä nestettä", 1);
        }
    }
}