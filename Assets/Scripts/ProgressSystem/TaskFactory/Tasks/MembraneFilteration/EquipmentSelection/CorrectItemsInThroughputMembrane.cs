using UnityEngine;
using System;
using System.Collections.Generic;

public class CorrectItemsInThroughputMembrane : TaskBase {
    #region Constants
    private const string DESCRIPTION = "Laita tarvittavat työvälineet läpiantokaappiin ja siirry työhuoneeseen.";
    private const string HINT = "Huoneessa on tarvittavat työvälineet pullot ja pipetti.";
    #endregion

    #region Fields
    public enum Conditions { Bottles100ml, PeptoniWaterBottle, SoycaseineBottle, TioglycolateBottle, Tweezers, Scalpel, Pipette, SoycaseinePlate, SabouradDextrosiPlate, Pump, PumpFilter, SterileBag }
    private int bottles100ml = 0;
    private int soycaseinePlate = 0;
    private int objectCount;
    private int correctItemCount = 14;
    private bool firstCheckDone = false;
    private CabinetBase cabinet;
    private OpenableDoor door;
    #endregion

    #region Constructor
    public CorrectItemsInThroughputMembrane() : base(TaskType.CorrectItemsInThroughputMembrane, true, false) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CorrectItems, EventType.RoomDoor);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase) data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.PassThrough) {
            this.cabinet = cabinet;
            door = cabinet.transform.Find("Door").GetComponent<OpenableDoor>();
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void CorrectItems(CallbackData data) {
        if ((DoorGoTo)data.DataObject != DoorGoTo.EnterWorkspace) {
            return;
        }

        if (cabinet == null) {
            Logger.Error("cabinet was null in CorrectItemsThroughputMembrane! That is weird.");
        }

        List<Interactable> containedObjects = cabinet.GetContainedItems();
        if (containedObjects.Count == 0) {
            Popup("Kerää tarvittavat työvälineet läpiantokaappiin.", MsgType.Notify);
            return;
        }

        int gCount = 0;

        foreach (Interactable obj in containedObjects) {
            
            gCount++;
            GeneralItem g = obj as GeneralItem;
            if ( g == null) {
                continue;
            }

            if (!g.IsClean) {
                if ((g.ObjectType == ObjectType.Bottle || g.ObjectType == ObjectType.Medicine) && g.Contamination == GeneralItem.ContaminateState.Contaminated) {
                    continue;
                }
                CreateTaskMistake("Läpiantokaapissa oli likainen esine", 1);
            }
        }

        if (gCount - correctItemCount > 0) { 
            int minus = gCount - correctItemCount;
            CreateTaskMistake("Läpiantokaapissa oli liikaa esineitä", minus);
        }

        objectCount = containedObjects.Count;
        CheckConditions(containedObjects);
        if (door.IsClosed) {

            CompleteTask();
            if (!IsCompleted()) {
                MissingItems();
            }
        } else {
            Popup("Sulje läpi-antokaapin ovi.", MsgType.Notify);
        }
    }
    #endregion

    private void MissingItems() {
        if (!firstCheckDone) {
            CreateTaskMistake("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", 2);
            firstCheckDone = true;
        } else {
            Popup("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", MsgType.Mistake);
        }
        //Logger.Print(cabinet.GetMissingItems());
        DisableConditions();
    }

    #region Private Methods
    private void CheckConditions(List<Interactable> containedObjects) {
        foreach (Interactable value in containedObjects) {
            GeneralItem item = value as GeneralItem;
            ObjectType type = item.ObjectType;
            Logger.Print("Condition: " + type);
            switch (type) {
                case ObjectType.Bottle:
                case ObjectType.Medicine:
                    bottles100ml++;
                    if (bottles100ml == 4) {
                        EnableCondition(Conditions.Bottles100ml);
                    }
                    break;
                case ObjectType.SoycaseinePlate:
                    soycaseinePlate++;
                    if (soycaseinePlate == 3)
                    {
                        EnableCondition(Conditions.SoycaseinePlate);
                    }
                    break;
                case ObjectType.SabouradDextrosiPlate:
                    EnableCondition(Conditions.SabouradDextrosiPlate);
                    break;
                case ObjectType.PeptoniWaterBottle:
                    EnableCondition(Conditions.PeptoniWaterBottle);
                    break;
                case ObjectType.SoycaseineBottle:
                    EnableCondition(Conditions.SoycaseineBottle);
                    break;
                case ObjectType.TioglycolateBottle:
                    EnableCondition(Conditions.TioglycolateBottle);
                    break;
                case ObjectType.Tweezers:
                    EnableCondition(Conditions.Tweezers);
                    break;
                case ObjectType.Scalpel:
                    EnableCondition(Conditions.Scalpel);
                    break;
                case ObjectType.Pipette:
                    EnableCondition(Conditions.Pipette);
                    break;
                case ObjectType.Pump:
                    EnableCondition(Conditions.Pump);
                    break;
                case ObjectType.PumpFilter:
                    EnableCondition(Conditions.PumpFilter);
                    break;
                case ObjectType.SterileBag:
                    EnableCondition(Conditions.SterileBag);
                    break;*/
            }
        }
    }

    

    protected override void OnTaskComplete() {
    }


    #endregion

    #region Public Methods
    public override void CompleteTask() {
        base.CompleteTask();

        if (IsCompleted()) {
            if (objectCount == correctItemCount) {
                Popup("Oikea määrä työvälineitä läpiantokaapissa.", MsgType.Done);
            }
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayerAndPassthroughCabinet();
            ((MedicinePreparationScene)G.Instance.Scene).InSecondRoom = true;
        }
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        string missingItemsHint = cabinet?.GetMissingItems() ?? "Kaikki";
        return "Tarkista välineitä läpiantokaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Huoneesta siirrytään pois tarttumalla oveen. Puuttuvat välineet: " + missingItemsHint;
    }
    #endregion
}