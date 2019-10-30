using UnityEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// Correct amount of items inserted into Throughput.
/// </summary>
public class CorrectItemsInThroughput : TaskBase {
    #region Fields
    public enum Conditions { BigSyringe, SmallSyringes, /*Needles, */Luerlock, RightSizeBottle }
    private int smallSyringes, needles;
    private int objectCount;
    private int checkTimes;
    private string description = "Laita valitsemasi työvälineet läpiantokaappiin ja siirry työhuoneeseen.";
    private CabinetBase cabinet;
    private OpenableDoor door;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectItemsInThroughput task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectItemsInThroughput() : base(TaskType.CorrectItemsInThroughput, true, false) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        SetItemsToZero();
        checkTimes = 0;
        points = 2;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        base.SubscribeEvent(CorrectItems, EventType.RoomDoor);
    }


    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.PassThrough) {
            this.cabinet = cabinet;
            door = cabinet.transform.Find("Door").GetComponent<OpenableDoor>();
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }
    }



    /// <summary>
    /// Once fired by an event, checks which items are in the throughput cabinet and sets the corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void CorrectItems(CallbackData data) {
        if ((DoorGoTo)data.DataObject != DoorGoTo.EnterWorkspace) {
            return;
        }
        if (cabinet == null) {
            return;
        }
        List<GameObject> containedObjects = cabinet.GetContainedItems();
        if (containedObjects.Count == 0) {
            return;
        }
        checkTimes++;
        objectCount = containedObjects.Count;

        CheckConditions(containedObjects);

        if (door.IsClosed) {
            bool check = CheckClearConditions(true);
            if (!check) {
                MissingItems(checkTimes);
            }
        } else {
            UISystem.Instance.CreatePopup("Sulje läpi-antokaapin ovi.", MessageType.Notify);
        }
    }
    #endregion

    #region Private Methods
    private void SetItemsToZero() {
        smallSyringes = 0;
        needles = 0;
    }

    private void CheckConditions(List<GameObject> containedObjects) {
        foreach (GameObject value in containedObjects) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            switch (type) {
                case ObjectType.Syringe:
                    Syringe syringe = item as Syringe;
                    if (syringe.Container.Capacity == 5000) {
                        EnableCondition(Conditions.BigSyringe);
                    } else if (syringe.Container.Capacity == 1000) {
                        smallSyringes++;
                        if (smallSyringes == 6) {
                            EnableCondition(Conditions.SmallSyringes);
                        }
                    }
                    break;
                /*case ObjectType.Needle:
                    needles++;
                    if (needles == 7) {
                        EnableCondition("Needles"); 
                    }
                    break;*/
                case ObjectType.Luerlock:
                    EnableCondition(Conditions.Luerlock);
                    break;
                case ObjectType.Bottle:
                    MedicineBottle bottle = item as MedicineBottle;
                    if (bottle.Container.Capacity == 80000) {
                        EnableCondition(Conditions.RightSizeBottle);
                    }
                    break;
            }
        }
    }

    private void MissingItems(int checkTimes) {
        if (checkTimes == 1) {
            UISystem.Instance.CreatePopup(0, "Välineitä puuttuu.", MessageType.Mistake);
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectItemsInThroughput, 2);
        } else {
            UISystem.Instance.CreatePopup("Välineitä puuttuu.", MessageType.Mistake);

        }
        //description = cabinet.GetMissingItems();
        //G.Instance.Progress.UpdateDescription();
        SetItemsToZero();
        DisableConditions();
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (checkTimes == 1) {
            //count changed from 16 to 9 for DEMO
            if (objectCount == 9) {
                UISystem.Instance.CreatePopup(2, "Oikea määrä työvälineitä.", MessageType.Notify);
            } else {
                UISystem.Instance.CreatePopup(1, "Liikaa työvälineitä.", MessageType.Notify);
                G.Instance.Progress.Calculator.Subtract(TaskType.CorrectItemsInThroughput);
            }
        }
        GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayerAndPassthroughCabinet();
        base.FinishTask();
    }

    public override string GetDescription() {
        return description;
    }

    public override string GetHint() {
        string missingItemsHint = cabinet.GetMissingItems();
        return "Tarkista välineitä läpiantokaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Huoneesta siirrytään pois tarttumalla oveen. " + missingItemsHint;
    }
    #endregion
}