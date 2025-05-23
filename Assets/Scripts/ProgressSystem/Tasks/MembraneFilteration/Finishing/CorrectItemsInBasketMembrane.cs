﻿using System.Collections.Generic;
using System;
using UnityEngine;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class CorrectItemsInBasketMembrane : Task {

    public enum Conditions { Bottles100ml, TioglycolateBottle, PeptoneWaterBottle, SoycaseineBottle, AgarPlate, Tweezers }
    private Basket basket;
    private bool cartMoved;
    private List<Interactable> alreadyChecked = new List<Interactable>();

    public CorrectItemsInBasketMembrane() : base(TaskType.CorrectItemsInBasketMembrane, false) {
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
            Logger.Error("Basket was null in CorrectItemsInBasketMembrane. Items not placed for reference.");
            return;
        }
        List<Interactable> containedObjects = basket.GetBasketItems();
        if (containedObjects.Count == 0) {
            Popup(Translator.Translate("XR MembraneFilteration 2.0", "CollectItemsToBasket"), MsgType.Notify);
            return;
        }
        CheckConditions(containedObjects);
        CompleteTask();
    }

    private void UpdateCheckedItems(List<Interactable> currentlyContained) {
        List<Interactable> newAlreadyChecked = new List<Interactable>();
        foreach (Interactable item in alreadyChecked) {
            if (currentlyContained.Contains(item)){
                newAlreadyChecked.Add(item);
            }
        }
        alreadyChecked = newAlreadyChecked;
    }
    private void CheckConditions(List<Interactable> containedObjects) {
        int bottles100ml = 0;
        int tioglycolateBottle = 0;
        int peptoneWaterBottle = 0;
        int soycaseineBottle = 0;
        int agarPlate = 0;

        foreach (var item in containedObjects) {
            if (Interactable.GetInteractable(item.transform) is var g && g != null) {
                if (g is Bottle bottle) {
                    int capacity = bottle.Container.Capacity;
                    LiquidType type = bottle.Container.LiquidType;
                    if (capacity == 100000) {
                        bottles100ml++;            
                        if (!alreadyChecked.Contains(item)) {
                            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                        }
                        if (bottles100ml == 4) {
                            EnableCondition(Conditions.Bottles100ml);
                        }
                    } else if (type == LiquidType.Peptonwater) {
                        peptoneWaterBottle++;
                        if (!alreadyChecked.Contains(item)) {
                            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                        }
                        EnableCondition(Conditions.PeptoneWaterBottle);
                    } else if (type == LiquidType.Soycaseine) {
                        soycaseineBottle++;
                        if (!alreadyChecked.Contains(item)) {
                            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                        }
                        EnableCondition(Conditions.SoycaseineBottle);
                    } else if (type == LiquidType.Tioglygolate) {
                        tioglycolateBottle++;
                        if (!alreadyChecked.Contains(item)) {
                            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                        }
                        EnableCondition(Conditions.TioglycolateBottle);
                    }
                }
                else if (g is AgarPlateLid agar) {
                    if (!agar.PlateBottom.GetComponent<AgarPlateBottom>().isOpen){
                        agarPlate++;
                        if (!alreadyChecked.Contains(item)) {
                            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
                        }
                    } else {
                        if (!alreadyChecked.Contains(item)) {
                        Popup(Translator.Translate("XR MembraneFilteration 2.0", "Close the plate"), MsgType.Notify);
                        }
                    }
                }
                else if (g is AgarPlateBottom agarB) {
                    if (agarB.GetComponent<AgarPlateBottom>().isOpen){
                        if (!alreadyChecked.Contains(item)) {
                        Popup(Translator.Translate("XR MembraneFilteration 2.0", "Close the plate"), MsgType.Notify);
                        }
                    }
                }
                else if(g is Tweezers){
                    EnableCondition(Conditions.Tweezers);
                }else if (g is BottleCap || g is FilteringButton || g is FilterHalf || g is FilterInCover || g is AgarPlateBottom || g is Agar) {
                    continue;
                } else {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongItemInBasket"), 1);
                }
                if (agarPlate >= 4) {
                    EnableCondition(Conditions.AgarPlate);
                }
            }
            alreadyChecked.Add(item);
        }
        UpdateCheckedItems(containedObjects);
        if (bottles100ml == 4 && peptoneWaterBottle == 1 && soycaseineBottle == 1 && tioglycolateBottle == 1 && agarPlate >= 4) {
            Logger.Print("All done");
        }
    }
}
