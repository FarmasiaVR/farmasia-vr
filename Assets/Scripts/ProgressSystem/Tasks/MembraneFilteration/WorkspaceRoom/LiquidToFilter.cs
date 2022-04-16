using System;

class LiquidToFilter : Task {

    public enum Conditions { AddedLiquid }
    
    private FilterPart pumpFilter;
    LiquidType liquidType;

    private readonly int REQUIRED_AMOUNT;

    public LiquidToFilter(string description, int amount, LiquidType liquid, TaskType taskType) : base(taskType, true) {
        this.description = description;
        liquidType = liquid;
        REQUIRED_AMOUNT = amount;
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        SubscribeEvent(OnFilterWet, EventType.TransferLiquidToBottle);
    }

    private void OnFilterWet(CallbackData data) {
        LiquidContainer container = data.DataObject as LiquidContainer;
        if (Started) {
            if (container.GeneralItem is FilterPart filter && filter.ObjectType == ObjectType.PumpFilterTank) {
                pumpFilter = filter;
                if (filter.Container.Amount >= REQUIRED_AMOUNT) {
                    EnableCondition(Conditions.AddedLiquid);
                    CheckMistakes();
                    CompleteTask();
                }
            }
        }/* else if(!package.doneTypes.Contains(TaskType.AssemblePump)){
            CreateGeneralMistake("Kiinnitä ensin pumppu filtteriin", 1);
        } else if (!package.doneTypes.Contains(TaskType.WetFilter) && !package.doneTypes.Contains(TaskType.StartPump)) {
            CreateGeneralMistake("Filtteröi ensin peptonivesi", 1);
        }*/
    }

    private void CheckMistakes() {
        if (pumpFilter.Container.LiquidType != liquidType && liquidType == LiquidType.Peptonwater) {
            CreateGeneralMistake("Et lisännyt peptonivettä filtteriin", 1);
        }
        if (pumpFilter.Container.LiquidType != liquidType && liquidType == LiquidType.Medicine) {
            CreateGeneralMistake("Et lisännyt lääkettä filtteriin", 1);
        }
        if (pumpFilter.Container.Amount > REQUIRED_AMOUNT) {
            CreateTaskMistake("Filtterissä on liikaa nestettä", 1);
        }
        if (pumpFilter.Container.Impure) {
            CreateTaskMistake("Filtterin neste on sekoittunut", 1);
        }
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup("Hienosti kostutettu!", MsgType.Done);
        }
    }
}