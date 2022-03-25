using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StartPump: Task {

    public enum Conditions { FilterIsEmpty }

    private PumpFilter pumpFilter;
    
    public StartPump() : base(TaskType.StartPump, true, false) {

        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        SubscribeEvent(OnFilterEmpty, EventType.FilterIsEmpty);
    }

    private void OnFilterEmpty(CallbackData data) {
        if (!Started) return;
        
        LiquidContainer container = data.DataObject as LiquidContainer;
        if (container.GeneralItem is PumpFilter filter && filter.ObjectType == ObjectType.PumpFilter) {
            pumpFilter = filter;
            if (filter.Container.Amount == 0) {
                EnableCondition(Conditions.FilterIsEmpty);
                CheckMistakes();
                CompleteTask();
            }
        }
    }

    private void CheckMistakes() {
        if (pumpFilter.Container.Amount > 0) {
            CreateTaskMistake("Suodattimessa on vielä nestett�", 1);
        }
    }

    protected override void OnTaskComplete() {

    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}