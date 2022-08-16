using System;
using System.Collections.Generic;
using UnityEngine;

public class CorrectItemsInBasketMembrane : Task {

    public enum Conditions {
        Bottles100ml, TioglycolateBottle, PeptoneWaterBottle, SoycaseineBottle, SoycaseinePlate, SabouraudDextrosePlate
    }
    private Basket basket;
    private Cart cart;
    private bool cartMoved;

    public CorrectItemsInBasketMembrane() : base(TaskType.CorrectItemsInBasketMembrane, false) {
        if (GameObject.Find("Cart") != null) cart = GameObject.Find("Cart").GetComponent<Cart>();
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(BringCartIn, EventType.ItemDroppedInTrash);
        base.SubscribeEvent(SetBasketReference, EventType.ItemPlacedBasketForReference);
        base.SubscribeEvent(CorrectItems, EventType.ItemPlacedInBasket);
    }

    private void BringCartIn(CallbackData data) {
        if (Started && !cartMoved) {
            cart.MoveCart();
            cartMoved = true;
        }
    }

    private void SetBasketReference(CallbackData data) {
        Basket basket = (Basket)data.DataObject;
        this.basket = basket;
        base.UnsubscribeEvent(SetBasketReference, EventType.ItemPlacedBasketForReference);
    }

    private void CorrectItems(CallbackData data) {
        if (basket == null) {
            Logger.Error("Basket was null in CorrectItemsInBasketMembrane. Items not placed for reference.");
            return;
        }
        List<Interactable> containedObjects = basket.GetBasketItems();
        if (containedObjects.Count == 0) {
            Popup("Kerää tarvittavat esineet koriin.", MsgType.Notify);
            return;
        }
        CheckConditions(containedObjects);
        CompleteTask();
    }

    private void CheckConditions(List<Interactable> containedObjects) {
        int bottles100ml = 0;
        int tioglycolateBottle = 0;
        int peptoneWaterBottle = 0;
        int soycaseineBottle = 0;
        int soycaseinePlate = 0;
        int sabouraudDextrosePlate = 0;

        foreach (var item in containedObjects) {
            if (Interactable.GetInteractable(item.transform) is var g && g != null) {
                if (g is Bottle bottle) {
                    int capacity = bottle.Container.Capacity;
                    LiquidType type = bottle.Container.LiquidType;
                    if (capacity == 100000) {
                        bottles100ml++;
                        if (bottles100ml == 4) {
                            EnableCondition(Conditions.Bottles100ml);
                        }
                    } else if (type == LiquidType.Peptonwater) {
                        peptoneWaterBottle++;
                        EnableCondition(Conditions.PeptoneWaterBottle);
                    } else if (type == LiquidType.Soycaseine) {
                        soycaseineBottle++;
                        EnableCondition(Conditions.SoycaseineBottle);
                    } else if (type == LiquidType.Tioglygolate) {
                        tioglycolateBottle++;
                        EnableCondition(Conditions.TioglycolateBottle);
                    }
                } else if (g is AgarPlateLid lid) {
                    string variant = lid.Variant;
                    if (variant == "Soija-kaseiini") {
                        soycaseinePlate++;
                        if (soycaseinePlate == 3) {
                            EnableCondition(Conditions.SoycaseinePlate);
                        }
                    } else if (variant == "Sabourad-dekstrosi") {
                        sabouraudDextrosePlate++;
                        EnableCondition(Conditions.SabouraudDextrosePlate);
                    }
                } else if (g is BottleCap || g is FilteringButton || g is FilterHalf || g is FilterInCover || g is AgarPlateBottom || g is Agar) {
                    continue;
                } else {
                    CreateTaskMistake("Väärä esine korissa", 1);
                }
            }
        }

        if (bottles100ml == 4 && peptoneWaterBottle == 1 && soycaseineBottle == 1 && tioglycolateBottle == 1 && soycaseinePlate == 3 && sabouraudDextrosePlate == 1) {
            Logger.Print("All done");
        }
    }
}
