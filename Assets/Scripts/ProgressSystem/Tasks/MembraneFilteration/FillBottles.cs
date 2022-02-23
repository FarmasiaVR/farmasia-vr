using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FillBottles: TaskBase {

    public enum Conditions { BottlesFilled }

    private int soycaseineBottlesDone = 0;
    private int tioglygolateBottlesDone = 0;

    private readonly int REQUIRED_AMOUNT = 100;

    public FillBottles() : base(TaskType.FillBottles, true, false) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        SubscribeEvent(OnBottleFill, EventType.TransferLiquidToBottle);
    }

    private void OnBottleFill(CallbackData data) {
        LiquidContainer container = data.DataObject as LiquidContainer;
        if (container.GeneralItem is MedicineBottle bottle && bottle.ObjectType == ObjectType.Bottle) {
            Logger.Print("Filling bottle, " + soycaseineBottlesDone + " " + tioglygolateBottlesDone);
            if (bottle.Container.Amount >= REQUIRED_AMOUNT) {
                if (bottle.Container.LiquidType == LiquidType.Soycaseine) {
                    soycaseineBottlesDone++;
                } else if (bottle.Container.LiquidType == LiquidType.Tioglygolate) {
                    tioglygolateBottlesDone++;
                }
            }
        }
        if (soycaseineBottlesDone >= 2 && tioglygolateBottlesDone >= 2) {
            EnableCondition(Conditions.BottlesFilled);
            CompleteTask();
        }
    }

    protected override void OnTaskComplete() {
        // juu
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (IsCompleted()) {
            Popup("Kova", MsgType.Done);
        }
    }

    public override string GetDescription() {
        return "Täytä pullot xd";
    }

    public override string GetHint() {
        return "Just do it";
    }
}
