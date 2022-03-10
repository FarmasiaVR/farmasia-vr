using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WetFilter : TaskBase {

    public enum Conditions { FilterIsWet }

    private PumpFilter pumpFilter;

    private readonly int REQUIRED_AMOUNT = 10000;

    public WetFilter() : base(TaskType.WetFilter, true, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        SubscribeEvent(OnFilterWet, EventType.TransferLiquidToBottle);
    }

    private void OnFilterWet(CallbackData data) {
        LiquidContainer container = data.DataObject as LiquidContainer;
        if (container.GeneralItem is PumpFilter filter && filter.ObjectType == ObjectType.PumpFilter) {
            pumpFilter = filter;
            if (filter.Container.Amount >= REQUIRED_AMOUNT) {
                EnableCondition(Conditions.FilterIsWet);
                CheckMistakes();
                CompleteTask();
            }
        }
    }

    private void CheckMistakes() {
        if (pumpFilter.Container.Amount > REQUIRED_AMOUNT) {
            CreateTaskMistake("Filtterissä on liikaa nestettä", 1);
        }
        if (pumpFilter.Container.Impure) {
            CreateTaskMistake("Filtterin neste on sekoittunut", 1);
        }
    }

    protected override void OnTaskComplete() {
        // juu
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (IsCompleted()) {
            Popup("Hienosti kostutettu!", MsgType.Done);
        }
    }

    public override string GetDescription() {
        return "Kostuta filtteri:D";
    }

    public override string GetHint() {
        return "Caman kyl sä osaat";
    }
}