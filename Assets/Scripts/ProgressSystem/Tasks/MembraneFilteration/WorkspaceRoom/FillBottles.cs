using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FillBottles: Task {

    public enum Conditions { BottlesFilled }

    private int soycaseineBottlesDone = 0;
    private int tioglygolateBottlesDone = 0;

    private readonly int REQUIRED_AMOUNT = 30000;

    private HashSet<Bottle> bottles = new HashSet<Bottle>();

    public FillBottles() : base(TaskType.FillBottles, false) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        SubscribeEvent(OnBottleFill, EventType.TransferLiquidToBottle);
    }

    private void OnBottleFill(CallbackData data) {
        LiquidContainer container = data.DataObject as LiquidContainer;
        if (container.GeneralItem is Bottle bottle && bottle.ObjectType == ObjectType.Bottle) {
            if (bottle.Container.Amount >= REQUIRED_AMOUNT) {
                if (bottles.Contains(bottle)) return;
                bottles.Add(bottle);
                if (bottle.Container.LiquidType == LiquidType.Soycaseine) {

                    soycaseineBottlesDone++;
                } else if (bottle.Container.LiquidType == LiquidType.Tioglygolate) {
                    tioglygolateBottlesDone++;
                }
            }
        }
        if (soycaseineBottlesDone >= 2 && tioglygolateBottlesDone >= 2) {
            EnableCondition(Conditions.BottlesFilled);
            CheckMistakes();
            CompleteTask();
        }
    }

    private void CheckMistakes() {
        foreach (var bottle in bottles) {

            var writable = bottle.GetComponent<Writable>();
            if (bottle.Container.LiquidType == LiquidType.Soycaseine) {

                if (!writable.WrittenLines.ContainsKey(WritingType.SoyCaseine)) {
                    CreateTaskMistake("Pulloon johon laitettiin soijakaseiinia, ei ole kirjoitettu 'Soijakaseiini'", 1);
                }
                if (bottle.Container.Amount > REQUIRED_AMOUNT) {
                    CreateTaskMistake("Soijakaseiinipullossa oli liikaa nestettä", 1);
                }
                if (bottle.Container.Impure) {
                    CreateTaskMistake("Soijakaseiinipullon neste oli sekoittunut", 1);
                }

            } else if (bottle.Container.LiquidType == LiquidType.Tioglygolate) {

                if (!writable.WrittenLines.ContainsKey(WritingType.Tioglygolate)) {
                    CreateTaskMistake("Pulloon johon laitettiin tioglykolaattia, ei ole kirjoitettu 'Tioglykolaatti'", 1);
                }
                if (bottle.Container.Amount > REQUIRED_AMOUNT) {
                    CreateTaskMistake("Tioglykolaattipullossa oli liikaa nestettä", 1);
                }
                if (bottle.Container.Impure) {
                    CreateTaskMistake("Tioglykolaattipullon neste oli sekoittunut", 1);
                }
            }

            
        }
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}
