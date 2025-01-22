using UnityEngine;
using System;
using System.Collections.Generic;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class CorrectItemsInThroughputMembrane: Task {

    #region Fields
    public enum Conditions {
        Bottles100ml, PeptoniWaterBottle, SoycaseineBottle, TioglycolateBottle, Tweezers, Scalpel, Pipette, SoycaseinePlate, SabouradDextrosiPlate, Pump, PumpFilter,
        SterileBag, PipetteHeads, BigPipette
    }
    private bool firstCheckDone = false;
    private CabinetBase cabinet;
    private OpenableDoor door;
    #endregion

    #region Constructor
    public CorrectItemsInThroughputMembrane() : base(TaskType.CorrectItemsInThroughputMembrane, false) {
        SetCheckAll(true);
        
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        Debug.Log("subcribed to events ");
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
        Debug.Log("Checking items");
        if ((DoorGoTo) data.DataObject != DoorGoTo.EnterWorkspace) {
            return;
        }

        if (cabinet == null) {
            Logger.Error("cabinet was null in CorrectItemsThroughputMembrane. Items not placed for reference");
            return;
        }

        List<Interactable> containedObjects = cabinet.GetContainedItems();
        if (containedObjects.Count == 0) {
            Popup(Translator.Translate("XR MembraneFilteration 2.0", "GetCorrectItemsThroughput"), MsgType.Notify);
            return;
        }

        if (containedObjects.Count > 44) {
            Logger.Print("ESINEIDEN MÄÄRÄ LÄPIANTOKAAPISSA: " + containedObjects.Count);
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "TooManyItemsThroughput"), 1);
        }

        foreach (Interactable obj in containedObjects) {

            GeneralItem g = obj as GeneralItem;
            if (g == null) {
                continue;
            }

            if (!g.IsClean) {
                if (g is Bottle) {
                    continue;
                } else {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "DirtyItemThroughput"), 1);
                }
            }
        }

        CheckConditions(containedObjects);

        if (door.IsClosed) {

            CompleteTask();
            if (!Completed) {
                MissingItems();
            }
        } else {
            Popup(Translator.Translate("XR MembraneFilteration 2.0", "CloseThroughput"), MsgType.Notify);
        }
    }
    #endregion

    private void MissingItems() {
        if (!firstCheckDone) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "MissingItemsThroughput"), 2);
            firstCheckDone = true;
        } else {
            Popup(Translator.Translate("XR MembraneFilteration 2.0", "MissingItemsThroughput"), MsgType.Mistake);
            
        }
        List<string> itemsNotInCabinet = new List<string>();
        base.GetNonClearedConditions().ForEach(c => {
            Logger.Print((Conditions) c);
            itemsNotInCabinet.Add(((Conditions)c).ToString());
        });
        cabinet.updateItemsNotInCabinet(itemsNotInCabinet);
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
        int pipetteHeads = 0;
        int bigPipette = 0;
        Debug.Log("Checking conditions of the passthrough cabinet in events:");
        foreach (var item in containedObjects) {
            if (Interactable.GetInteractable(item.transform) is var g && g != null) {
                if (g is Bottle bottle) {
                    int capacity = bottle.Container.Capacity;
                    LiquidType type = bottle.Container.LiquidType;
                    if (capacity == 100000) {
                        // Debug.Log("detected bottle 100ml entering cabinet");
                        bottles100ml++;
                        if (bottles100ml == 4) {
                            EnableCondition(Conditions.Bottles100ml);
                        }

                    } else if (type == LiquidType.Peptonwater) {
                        // Debug.Log("detected peptone water entering cabinet");
                        peptonWaterBottle++;
                        EnableCondition(Conditions.PeptoniWaterBottle);
                    } else if (type == LiquidType.Soycaseine) {
                        // Debug.Log("detected soycaseine bottle entering cabinet");
                        soycaseineBottle++;
                        EnableCondition(Conditions.SoycaseineBottle);
                    } else if (type == LiquidType.Tioglygolate) {
                        // Debug.Log("detected Tioglygolate entering cabinet");
                        tioglycolateBottle++;
                        EnableCondition(Conditions.TioglycolateBottle);
                    } else {
                        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongBottleThroughput"), 5);
                    }
                } else if (g is AgarPlateLid lid) {
                    string variant = lid.Variant;
                    if (variant == "Soija-kaseiini") {
                        // Debug.Log("detected soija kaseiini entering cabinet");
                        soycaseinePlate++;
                        if (soycaseinePlate == 3) {
                            EnableCondition(Conditions.SoycaseinePlate);
                        }
                    } else if (variant == "Sabourad-dekstrosi") {
                        // Debug.Log("detected Sabourad-dekstrosi plate entering cabinet");
                        sabouradDextrosiPlate++;
                        EnableCondition(Conditions.SabouradDextrosiPlate);
                    } else {
                        CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongAgarPlateThroughput"), 5);
                    }

                } else if (g is Tweezers) {
                    // Debug.Log("detected tweezers entering cabinet");
                    EnableCondition(Conditions.Tweezers);
                    tweezers++;
                } else if (g is Scalpel) {
                    // Debug.Log("detected scalpel entering cabinet");
                    EnableCondition(Conditions.Scalpel);
                    scalpel++;
                } else if (g is Pipette) {
                    // Debug.Log("detected pipette entering cabinet");
                    pipette++;
                    if (pipette == 1) {
                        EnableCondition(Conditions.Pipette);
                    }
                } else if (g is Pump) {
                    // Debug.Log("detected pump entering cabinet");
                    EnableCondition(Conditions.Pump);
                    pump++;
                } else if (g is FilterInCover) {
                    // Debug.Log("detected filter in cover entering cabinet");
                    EnableCondition(Conditions.PumpFilter);
                    filter++;
                
                } else if ((g is SterileBag2) || (g is SterileBag)) {
                    // Debug.Log("detected SterileBag2 entering cabinet");
                    EnableCondition(Conditions.SterileBag);
                    sterileBag++;
                } else if (g is PipetteHeadCover) {
                    // Debug.Log("detected pipetteheadincover entering cabinet");
                    pipetteHeads++;
                    if (pipetteHeads == 2) {
                        EnableCondition(Conditions.PipetteHeads);
                    }
                } else if (g is BigPipette) {
                    // Debug.Log("detected bigpipette entering cabinet");
                    EnableCondition(Conditions.BigPipette);
                    bigPipette++;
                }
            }
        }
        if (!(bottles100ml == 4 && peptonWaterBottle == 1 && soycaseineBottle == 1 && tioglycolateBottle == 1 && soycaseinePlate == 3 && sabouradDextrosiPlate == 1 && tweezers == 1 && scalpel == 1 && pipette == 3 && pump == 1 && filter == 1 && sterileBag == 1)) {
            // CreateTaskMistake("Väärä määrä työvälineitä läpiantokaapissa.", 2);
        }
    }


    #endregion

    #region Public Methods
    public override void CompleteTask() {
        base.CompleteTask();

        if (Completed) {
            
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
        }
    }
    #endregion
}