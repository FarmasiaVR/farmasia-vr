using System;
using System.Collections.Generic;
using UnityEngine;

public class CorrectItemsInBasketMembrane : Task {

    public enum Conditions {
        Bottles100ml, SoycaseinePlate, SabouradDextrosiPlate, Pump
    }
    private Basket basket;
    private bool firstCheckDone = false;

    public CorrectItemsInBasketMembrane() : base(TaskType.CorrectItemsInBasketMembrane, true) {
        SetCheckAll(true);

        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(SetBasketReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CorrectItems, EventType.RoomDoor);
    }

    private void SetBasketReference(CallbackData data)
    {
        Basket basket = (Basket)data.DataObject;
        this.basket = basket;
        base.UnsubscribeEvent(SetBasketReference, EventType.ItemPlacedForReference);
    }

    private void CorrectItems(CallbackData data)
    {
        if (basket == null)
        {
            Logger.Error("Basket was null in CorrectItemsInBasketMembrane. Items not placed for reference.");
            return;
        }

        List<Interactable> containedObjects = basket.GetBasketItems();
        if (containedObjects.Count == 0)
        {
            Popup("Kerää tarvittavat työvälineet koriin.", MsgType.Notify);
            return;
        }
        if (containedObjects.Count > 9)
        {
            Logger.Print("ESINEIDEN MÄÄRÄ KORISSA: " + containedObjects.Count);
            CreateTaskMistake("Korissa oli liikaa esineitä", 1);
        }
        CheckConditions(containedObjects);

        CompleteTask();
        if (!Completed)
        {
            MissingItems();
        }

    }

    private void MissingItems()
    {
        if (!firstCheckDone)
        {
            CreateTaskMistake("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", 2);
            firstCheckDone = true;
        }
        else
        {
            Popup("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", MsgType.Mistake);
        }
        base.GetNonClearedConditions().ForEach(c =>
        {
            Logger.Print((Conditions)c);
        });
        DisableConditions();
    }

    private void CheckConditions(List<Interactable> containedObjects)
    {
        int bottles100ml = 0;
        int soycaseinePlate = 0;
        int sabouradDextrosiPlate = 0;
        int pump = 0;

        foreach (var item in containedObjects)
        {
            if (Interactable.GetInteractable(item.transform) is var g && g != null)
            {
                if (g is Bottle bottle)
                {
                    bottles100ml++;
                    if (bottles100ml == 4)
                    {
                        EnableCondition(Conditions.Bottles100ml);
                    }
                }
                else if (g is AgarPlateLid lid)
                {
                    string variant = lid.Variant;
                    if (variant == "Soija-kaseiini")
                    {
                        soycaseinePlate++;
                        if (soycaseinePlate == 3)
                        {
                            EnableCondition(Conditions.SoycaseinePlate);
                        }
                    }
                    else if (variant == "Sabourad-dekstrosi")
                    {
                        sabouradDextrosiPlate++;
                        EnableCondition(Conditions.SabouradDextrosiPlate);
                    }
                    else if (g is Pump)
                    {
                        pump++;
                        EnableCondition(Conditions.Pump);
                    }
                    else
                    {
                        CreateTaskMistake("Väärä asia korissa", 1);
                    }
                }
            }
        }
        if (!(bottles100ml == 4 && soycaseinePlate == 3 && sabouradDextrosiPlate == 1 && pump == 1))
        {
            CreateTaskMistake("Väärä määrä työvälineitä korissa.", 1);
        }
    }
    public override void CompleteTask()
    {
        base.CompleteTask();
    }
}