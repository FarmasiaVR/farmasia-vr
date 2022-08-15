using System.Collections.Generic;
using System;
using UnityEngine;

public class CorrectItemsInThroughputMedicine : Task {

    public enum Conditions { MedicineBottle, BigSyringe, SmallSyringe, SyringeCapBag, Needle, Luerlock }
    private CabinetBase cabinet;
    private OpenableDoor door;
    private bool firstCheckDone = false;

    public CorrectItemsInThroughputMedicine() : base(TaskType.CorrectItemsInThroughputMedicine, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        base.SubscribeEvent(CheckItems, EventType.RoomDoor);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.PassThrough) {
            this.cabinet = cabinet;
            door = cabinet.transform.Find("Door").GetComponent<OpenableDoor>();
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void CheckItems(CallbackData data) {
        if (cabinet == null) {
            Logger.Error("Cabinet was null in CorrectItemsInThroughputMedicine");
            return;
        }
        List<Interactable> containedObjects = cabinet.GetContainedItems();
        if (containedObjects.Count > 11) {
            CreateTaskMistake("Läpiantokaapissa oli liikaa esineitä.", 1);
        }
        CheckConditions(containedObjects);
        if (door.IsClosed) {
            CompleteTask();
            if (!Completed) {
                if (!firstCheckDone) {
                    CreateTaskMistake("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", 2);
                    firstCheckDone = true;
                } else {
                    Popup("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", MsgType.Mistake);
                }
                DisableConditions();
            }
        } else {
            Popup("Sulje läpiantokaapin ovi.", MsgType.Notify);
        }
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

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
        }
    }
}
