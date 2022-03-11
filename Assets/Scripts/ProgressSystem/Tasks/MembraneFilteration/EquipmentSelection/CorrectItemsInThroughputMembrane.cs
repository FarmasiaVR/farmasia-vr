using UnityEngine;
using System;
using System.Collections.Generic;

public class CorrectItemsInThroughputMembrane : Task {
    #region Constants
    public new string Description = "Laita tarvittavat työvälineet läpiantokaappiin ja siirry työhuoneeseen.";
    private const string HINT = "Huoneessa on tarvittavat työvälineet pullot ja pipetti. \n" +
        "Mediumit = Soijakaseiini-pullo ja Tioglygolaattipullo. \n\n" +
        "Sormenpäämaljat ja toinen laskeumamalja ovat soijakaseiinimaljoja. \n" +
        "Toinen laskeumamalja on sabourad-dekstroosimalja.";
    #endregion

    #region Fields
    public enum Conditions { Bottles100ml, PeptoniWaterBottle, SoycaseineBottle, TioglycolateBottle, Tweezers, Scalpel, Pipette, SoycaseinePlate, SabouradDextrosiPlate, Pump, PumpFilter, SterileBag, CleaningBottle}
    private bool firstCheckDone = false;
    private CabinetBase cabinet;
    private OpenableDoor door;
    #endregion

    #region Constructor
    public CorrectItemsInThroughputMembrane() : base(TaskType.CorrectItemsInThroughputMembrane, true, false) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        Points = 2;
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
            return;
        }

        List<Interactable> containedObjects = cabinet.GetContainedItems();
        if (containedObjects.Count == 0) {
            Popup("Kerää tarvittavat työvälineet läpiantokaappiin.", MsgType.Notify);
            return;
        }

        foreach (Interactable obj in containedObjects) {            
  
            GeneralItem g = obj as GeneralItem;
            if ( g == null) {
                continue;
            }

            if (!g.IsClean) {
                if (g is Bottle) {
                    continue;
                }
                CreateTaskMistake("Läpiantokaapissa oli likainen esine", 1);
            }
        }
   
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
    #endregion

    private void MissingItems() {
        if (!firstCheckDone) {
            CreateTaskMistake("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", 2);
            firstCheckDone = true;
        } else {
            Popup("Työvälineitä puuttuu tai sinulla ei ole oikeita työvälineitä.", MsgType.Mistake);
        }
        DisableConditions();
    }

    #region Private Methods
    private void CheckConditions(List<Interactable> containedObjects) {
        int bottles100ml = 0;
        int peptonWaterBottle = 0;
        int soycaseineBottle = 0;
        int tioglycolateBottle = 0;
        int soycaseinePlate = 0;
        int sabouradDextrosiPlate = 0;
        int tweezers = 0;
        int scalpel = 0;
        int pipette = 0;
        int sterileBag = 0;
        int pump = 0;
        int filter = 0;
        int cleaningBottle = 0;

        foreach (var item in containedObjects)
        {
            if (Interactable.GetInteractable(item.transform) is var g && g != null)
            {
                if (g is Bottle bottle)
                {
                    int capacity = bottle.Container.Capacity;
                    LiquidType type = bottle.Container.LiquidType;
                    if (capacity == 100000)
                    {
                        bottles100ml++;
                        if (bottles100ml == 4)
                        {
                            EnableCondition(Conditions.Bottles100ml);
                        }

                    }
                    else if (type == LiquidType.Peptonwater)
                    {
                        peptonWaterBottle++;
                        EnableCondition(Conditions.PeptoniWaterBottle);
                    }
                    else if (type == LiquidType.Soycaseine)
                    {
                        soycaseineBottle++;
                        EnableCondition(Conditions.SoycaseineBottle);
                    }
                    else if (type == LiquidType.Tioglygolate)
                    {
                        tioglycolateBottle++;
                        EnableCondition(Conditions.TioglycolateBottle);
                    }
                    else
                    {
                        CreateTaskMistake("Väärä pullo laminaarikaapissa", 5);
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
                    else
                    {
                        CreateTaskMistake("Väärä agarmalja laminaarikaapissa", 5);
                    }

                }
                else if (g is Tweezers)
                {
                    EnableCondition(Conditions.Tweezers);
                    tweezers++;
                }
                else if (g is Scalpel)
                {
                    EnableCondition(Conditions.Scalpel);
                    scalpel++;
                }
                else if (g is Pipette || g is BigPipette)
                {
                    pipette++;
                    if (pipette == 3)
                    {
                        EnableCondition(Conditions.Pipette);
                    }                  
                }
                else if (g is Pump)
                {
                    EnableCondition(Conditions.Pump);
                    pump++;
                }
                else if (g is PumpFilter)
                {
                    EnableCondition(Conditions.PumpFilter);
                    filter++;
                }
                else if (g is SterileBag)
                {
                    EnableCondition(Conditions.SterileBag);
                    sterileBag++;
                }
                else if (g is CleaningBottle)
                {
                    EnableCondition(Conditions.CleaningBottle);
                    cleaningBottle++;
                }
            }
        }
        if (!(bottles100ml == 4 && peptonWaterBottle == 1 && soycaseineBottle == 1 && tioglycolateBottle == 1 && soycaseinePlate == 3 && sabouradDextrosiPlate == 1 && tweezers == 1 && scalpel == 1 && pipette == 3 && pump == 1 && filter == 1 && sterileBag == 1 && cleaningBottle == 1))
        {
            CreateTaskMistake("Väärä määrä työvälineitä laminaarikaapissa.", 2);
        }        
    }
    

    protected override void OnTaskComplete() {
    }


    #endregion

    #region Public Methods
    public override void CompleteTask() {
        base.CompleteTask();

        if (Completed) {
            Popup("Oikeat työvälineet läpiantokaapissa.", MsgType.Done);            
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayerAndPassthroughCabinet();
            ((MedicinePreparationScene)G.Instance.Scene).InSecondRoom = true;
        }
    }

    public override string GetHint() {
        return HINT;
    }
    #endregion
}