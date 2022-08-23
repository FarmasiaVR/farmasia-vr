using System.Collections.Generic;
using System;

public class CorrectItemsInLaminarCabinetMedicine : Task {

    public enum Conditions { MedicineBottle, BigSyringe, SmallSyringe, SyringeCapBag, Needle, Luerlock }
    private CabinetBase cabinet;

    public CorrectItemsInLaminarCabinetMedicine() : base(TaskType.CorrectItemsInLaminarCabinetMedicine, false) {
        SetCheckAll(true);
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CheckItems, EventType.CheckLaminarCabinetItems);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            this.cabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void CheckItems(CallbackData data) {
        if (cabinet == null) {
            return;
        }
        List<Interactable> containedObjects = cabinet.GetContainedItems();
        CheckConditions(containedObjects);
        CompleteTask();
    } 

    private void CheckConditions(List<Interactable> containedObjects) {
        int smallSyringe = 0;
        foreach (Interactable item in containedObjects) {
            if (Interactable.GetInteractable(item.transform) is var g && g != null) {
                if (g is Bottle bottle) {
                    if (bottle.Container.Capacity == 4000) {
                        EnableCondition(Conditions.MedicineBottle);
                    }
                } else if (g is Syringe syringe) {
                    int capacity = syringe.Container.Capacity;
                    if (capacity == 20000) {
                        EnableCondition(Conditions.BigSyringe);
                    } else if (capacity == 1000) {
                        smallSyringe++;
                        if (smallSyringe == 6) {
                            EnableCondition(Conditions.SmallSyringe);
                        }
                    }
                } else if (g is SyringeCapBag) {
                    EnableCondition(Conditions.SyringeCapBag);
                } else if (g is Needle) {
                    EnableCondition(Conditions.Needle);
                } else if (g is LuerlockAdapter) {
                    EnableCondition(Conditions.Luerlock);
                }
            }
        }
    }
}
