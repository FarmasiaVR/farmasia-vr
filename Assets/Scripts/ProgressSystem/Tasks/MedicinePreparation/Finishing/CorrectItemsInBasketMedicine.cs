using System;

public class CorrectItemsInBasketMedicine : Task {

    public enum Conditions { }

    public CorrectItemsInBasketMedicine() : base(TaskType.CorrectItemsInBasketMedicine, false) {
        SetCheckAll(true);

        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {

    }
}
