using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class WetFilter : Task {

    public enum Conditions { FilterIsWet }

    private PumpFilter pumpFilter;

    private readonly int REQUIRED_AMOUNT = 10000;

    public WetFilter() : base(TaskType.WetFilter, true, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        SubscribeEvent(OnFilterWet, EventType.TransferLiquidToBottle);
        SubscribeEvent(OnFilterDissassemble, EventType.FilterDissassembled);
    }

    private void OnFilterWet(CallbackData data) {
        if (!Started) return;
        
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

    private void OnFilterDissassemble(CallbackData data) {
        // The data of the event 'FilterDissassembled' holds both parts that are disconnected
        var (bottom, top) = data.DataObject as Tuple<FilterPart, FilterPart>;
        // We're interested in these cases:
        // PumpFilterBase <-> PumpFilterFilter
        // PumpFilterFilter <-> PumpFilterTank
        LiquidContainer container = null;
        if (bottom.ObjectType == ObjectType.PumpFilterBase) {
            container = (top.ConnectedItem as FilterPart)?.Container;
        } else if (bottom.ObjectType == ObjectType.PumpFilterFilter) {
            container = (top as FilterPart)?.Container;
        }
        if (container == null) return;

        if (container.Amount != 0) {
            CreateGeneralMistake("Avasit pumpun filtterin kun siinä oli vielä nestettä!");
        }
    }

    private void CheckMistakes() {
        if (pumpFilter.Container.Amount > REQUIRED_AMOUNT) {
            CreateTaskMistake("Filtteriss� on liikaa nestett�", 1);
        }
        if (pumpFilter.Container.Impure) {
            CreateTaskMistake("Filtterin neste on sekoittunut", 1);
        }
    }

    public override void CompleteTask() {
        base.CompleteTask();
        var successText = TaskConfig.For(TaskType).Success;
        if (Completed && successText != null) {
            Popup(successText, MsgType.Done, base.Points);
        }
    }
}