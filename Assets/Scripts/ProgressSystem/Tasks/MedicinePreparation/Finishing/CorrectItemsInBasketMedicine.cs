using System.Collections.Generic;
using System;
using UnityEngine;
using FarmasiaVR.Legacy;

public class CorrectItemsInBasketMedicine : Task {

    public enum Conditions { SterileBag, MedicineBottle }
    private Basket basket;
    private bool cartMoved;

    public CorrectItemsInBasketMedicine() : base(TaskType.CorrectItemsInBasketMedicine, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(BringCartIn, EventType.ItemDroppedInTrash);
        base.SubscribeEvent(SetBasketReference, EventType.ItemPlacedBasketForReference);
        base.SubscribeEvent(CorrectItems, EventType.ItemPlacedInBasket);
    }

    private void BringCartIn(CallbackData data) {
        Cart cart = GameObject.Find("Cart").GetComponent<Cart>();
        if (Started && cart != null && !cartMoved) {
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
            Logger.Error("Basket was null in CorrectItemsInBasketMedicine");
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
        foreach (var item in containedObjects) {
            if (Interactable.GetInteractable(item.transform) is var g && g != null) {
                if (g is SterileBag) {
                    EnableCondition(Conditions.SterileBag);
                } else if (g is Bottle) {
                    EnableCondition(Conditions.MedicineBottle);
                } else if (g is BottleCap || g is FilteringButton || g is FilterHalf || g is FilterInCover || g is AgarPlateBottom || g is Agar) {
                    continue;
                } else {
                    CreateTaskMistake("Väärä esine korissa", 1);
                }
            }
        }
    }
}
