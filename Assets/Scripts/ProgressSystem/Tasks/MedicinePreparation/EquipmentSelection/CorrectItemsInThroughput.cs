using UnityEngine;
using System;
using System.Collections.Generic;

public class CorrectItemsInThroughput : Task {

    public enum Conditions { BigSyringe, SmallSyringes, Needle, Luerlock, SyringeCapBag, RightBottle }
    private int smallSyringes = 0;
    private int objectCount;
    private bool firstCheckDone = false;
    private CabinetBase cabinet;
    private OpenableDoor door;

    public CorrectItemsInThroughput() : base(TaskType.CorrectItemsInThroughput, false) {
        SetCheckAll(true);
        
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }


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
            Popup("Kerää tarvittavat työvälineet läpiantokaappiin.", MsgType.Notify);
            return;
        }
        List<Interactable> containedObjects = cabinet.GetContainedItems();
        if (containedObjects.Count == 0) {
            Popup("Kerää tarvittavat työvälineet läpiantokaappiin.", MsgType.Notify);
            return;
        }

        int gCount = 0;

        foreach (Interactable obj in containedObjects) {
            if (obj is Bottle) {
                continue;
            }
            GeneralItem g = obj as GeneralItem;
            if ( g == null) {
                continue;
            }

            if (!g.IsClean) {
                Logger.Warning(g.name + " in throughput cabinet was not clean");
                CreateTaskMistake("Läpiantokaapissa oli likainen esine", 1);
            }
        }

        if (gCount - 11 > 0) {
            int minus = gCount - 11;
            CreateTaskMistake("Läpiantokaapissa oli liikaa esineitä", minus);
        }

        objectCount = containedObjects.Count;
        CheckConditions(containedObjects);
        if (door.IsClosed) {
            CompleteTask();
            if (!Completed) {
                MissingItems();
            }
        } else {
            Popup("Sulje läpi-antokaapin ovi.", MsgType.Notify);
        }
    }


    private void MissingItems() {
        if (!firstCheckDone) {
            CreateTaskMistake("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", 2);
            Logger.Print("object count: " + objectCount + ", small syringe count: " + smallSyringes);
            firstCheckDone = true;
        } else {
            Popup("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", MsgType.Mistake);
        }
        //Logger.Print(cabinet.GetMissingItems());
        smallSyringes = 0;
        DisableConditions();
    }

    private void CheckConditions(List<Interactable> containedObjects) {
        foreach (Interactable value in containedObjects) {
            GeneralItem item = value as GeneralItem;
            ObjectType type = item.ObjectType;
            switch (type) {
                case ObjectType.Syringe:
                    Syringe syringe = item as Syringe;
                    if (syringe.Container.Capacity == 20000) {
                        EnableCondition(Conditions.BigSyringe);
                    } else if (syringe.Container.Capacity == 1000) {
                        smallSyringes++;
                        if (smallSyringes == 6) {
                            EnableCondition(Conditions.SmallSyringes);
                        }
                    }
                    break;
                case ObjectType.Needle:
                    EnableCondition(Conditions.Needle);
                    break;
                case ObjectType.Luerlock:
                    EnableCondition(Conditions.Luerlock);
                    break;
                case ObjectType.SyringeCapBag:
                    EnableCondition(Conditions.SyringeCapBag);
                    break;
                case ObjectType.Bottle:
                case ObjectType.Medicine:
                    Bottle bottle = item as Bottle;
                    if (bottle.Container.Capacity == 4000 || bottle.Container.Capacity == 16000) {
                        EnableCondition(Conditions.RightBottle);
                    }
                    break;
            }
        }
    }



    public override void CompleteTask() {
        base.CompleteTask();

        if (Completed) {
            if (objectCount == 11) {
                Popup(base.success, MsgType.Done);
            }
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayerAndPassthroughCabinet();
            ((MedicinePreparationScene)G.Instance.Scene).InSecondRoom = true;
        }
    }

    public override string Hint { 
        get {
        string missingItemsHint = cabinet?.GetMissingItems() ?? "Kaikki";
        return base.hint + missingItemsHint;
        }
    }
}