using System;
using UnityEngine;

public class Finish : TaskBase {

    #region Constants
    private const int RIGHT_SMALL_SYRINGE_CAPACITY = 1000;
    private const int MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 140;
    private const int MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 160;

    private const string DESCRIPTION = "Siirry pois työtilasta.";
    private const string HINT = "Siirry pois työtilasta tarttumalla ovenkahvaan.";
    #endregion

    #region Fields
    private CabinetBase laminarCabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for Finish task.
    ///  Is not removed when finished and requires previous task completion.
    ///  </summary>
    public Finish() : base(TaskType.Finish, false, true) {
        Subscribe();
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        SubscribeEvent(DoorHandleGrabbed, EventType.RoomDoor);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }        
    }

    private void DoorHandleGrabbed(CallbackData data) {
        if ((DoorGoTo)data.DataObject != DoorGoTo.Exit) {
            return;
        }
        if (!G.Instance.Progress.IsCurrentPackage(PackageName.CleanUp)) {
            UISystem.Instance.CreatePopup("Suorita laminaarikaapin tehtävät ennen pelin päättämistä.", MsgType.Mistake);
            AudioManager.Instance.Play("mistakeMessage");
        } else {
            FinishTask();
        }
    }
    #endregion

    #region Private Methods
    private void PointsForSmallSyringes() {
        int pointsForSyringeSize = 0;
        int pointsForMedicineAmount = 0;
        foreach (GameObject value in laminarCabinet.objectsInsideArea) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            if (type == ObjectType.Syringe) {
                Syringe s = item.GetComponent<Syringe>();
                if (s.Container.Capacity == RIGHT_SMALL_SYRINGE_CAPACITY && !s.hasBeenInBottle && s.Container.Amount > 0) {
                    pointsForSyringeSize = Math.Min(6, pointsForSyringeSize++);
                    if (s.Container.Amount >= MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && s.Container.Amount <= MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
                        pointsForMedicineAmount = Math.Min(6, pointsForMedicineAmount++);
                    }
                }
            }   
        }
        
        if (pointsForSyringeSize < 6) {
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.SyringeAttach, 6 - pointsForSyringeSize);
        }
        if (pointsForMedicineAmount < 6) {
            G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectAmountOfMedicineSelected, 6 - pointsForMedicineAmount);
        }
    }

    private void IsSterileBagTaskFinished() {
        foreach (Package p in G.Instance.Progress.packages) {
            if (p.name == PackageName.Workspace) {
                if (!p.doneTypes.Contains(TaskType.ItemsToSterileBag)) {
                    G.Instance.Progress.Calculator.Subtract(TaskType.ItemsToSterileBag); 
                }
                break;
            }
        }
    }

    private void DroppedItemsCleaned() {
        foreach (Package p in G.Instance.Progress.packages) {
            if (p.name == PackageName.CleanUp) {
                if (!p.doneTypes.Contains(TaskType.ScenarioOneCleanUp)) {
                    if (G.Instance.Progress.FindTaskWithType(TaskType.ScenarioOneCleanUp) != null) {
                        G.Instance.Progress.FindTaskWithType(TaskType.ScenarioOneCleanUp).FinishTask();
                    } else {
                        foreach (ITask task in p.activeTasks) {
                            if (task.GetTaskType() == TaskType.ScenarioOneCleanUp) {
                                task.FinishTask();
                                break;
                            }
                        }
                    } 
                }
                break;
            }
        }
    }

    private void LayoutInThroughPut() {
        ITask layoutThroughPut = G.Instance.Progress.FindTaskWithType(TaskType.CorrectLayoutInThroughput);
        layoutThroughPut.FinishTask();
    }

    private void LayoutInLaminarCabinet() {
        ITask layoutLaminarCabinet = G.Instance.Progress.FindTaskWithType(TaskType.CorrectLayoutInLaminarCabinet);
        layoutLaminarCabinet.FinishTask();
    }

    private void BottlesDisinfected() {
        ITask disinfect = G.Instance.Progress.FindTaskWithType(TaskType.DisinfectBottles);
        disinfect.FinishTask();
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Onnittelut!\nPeli päättyi.", MsgType.Done);
        
        PointsForSmallSyringes();
        IsSterileBagTaskFinished();
        DroppedItemsCleaned();
        LayoutInThroughPut();
        LayoutInLaminarCabinet();
        BottlesDisinfected();
        
        base.FinishTask();
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }
    #endregion
}
