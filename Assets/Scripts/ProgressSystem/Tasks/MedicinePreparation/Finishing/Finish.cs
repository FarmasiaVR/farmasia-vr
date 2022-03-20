using System;
using System.Threading.Tasks;
using UnityEngine;

public class Finish : Task {

    #region Constants
    private const int RIGHT_SMALL_SYRINGE_CAPACITY = 1000;
    private const int MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 140;
    private const int MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE = 160;

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
        SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }
    #endregion

    #region Private Methods
    private void PointsForSmallSyringes() {
        int pointsForSyringeSize = 0;
        int pointsForMedicineAmount = 0;
        foreach (Interactable value in laminarCabinet.GetContainedItems()) {
            GeneralItem item = value as GeneralItem;
            if (item == null) {
                return;
            }
            ObjectType type = item.ObjectType;
            if (type == ObjectType.Syringe) {
                Syringe s = item.GetComponent<Syringe>();
                if (s.Container.Capacity == RIGHT_SMALL_SYRINGE_CAPACITY && !s.hasBeenInBottle && s.Container.Amount > 0) {
                    int points = pointsForSyringeSize + 1;
                    pointsForSyringeSize = Math.Min(6, points);
                    if (s.Container.Amount >= MINIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE && s.Container.Amount <= MAXIMUM_CORRECT_AMOUNT_IN_SMALL_SYRINGE) {
                        points = pointsForMedicineAmount + 1;
                        pointsForMedicineAmount = Math.Min(6, points);
                    }
                }
            }
        }

        if (pointsForSyringeSize < 6) {
            Logger.Error("Deprecated finish minus");
            //G.Instance.Progress.Calculator.SubtractWithScore(TaskType.SyringeAttach, 6 - pointsForSyringeSize);
        }
        if (pointsForMedicineAmount < 6) {
            Logger.Error("Deprecated finish minus");
            //G.Instance.Progress.Calculator.SubtractWithScore(TaskType.CorrectAmountOfMedicineSelected, 6 - pointsForMedicineAmount);
        }
    }

    private void IsSterileBagTaskFinished() {
        foreach (Package p in G.Instance.Progress.packages) {
            if (p.name == PackageName.Workspace) {
                if (!p.doneTypes.Contains(TaskType.ItemsToSterileBag)) {
                    G.Instance.Progress.FindTaskWithType(TaskType.ItemsToSterileBag).FinishTask();
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
                        foreach (Task task in p.activeTasks) {
                            if (task.TaskType == TaskType.ScenarioOneCleanUp) {
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
        Task layoutThroughPut = G.Instance.Progress.FindTaskWithType(TaskType.CorrectLayoutInThroughput);
        layoutThroughPut.FinishTask();
    }

    private void LayoutInLaminarCabinet() {
        Task layoutLaminarCabinet = G.Instance.Progress.FindTaskWithType(TaskType.CorrectLayoutInLaminarCabinet);
        layoutLaminarCabinet.FinishTask();
    }

    private void BottlesDisinfected() {
        Task disinfect = G.Instance.Progress.FindTaskWithType(TaskType.DisinfectBottles);
        disinfect.FinishTask();
    }
    #endregion

    #region Overridden Methods
    public override void StartTask() {
        if (!Started) {
            FinishTask();
        }
        base.StartTask();
    }
    #endregion

    #region Public Methods
    public override async void FinishTask() {


        PointsForSmallSyringes();
        IsSterileBagTaskFinished();
        DroppedItemsCleaned();
        LayoutInThroughPut();
        LayoutInLaminarCabinet();
        BottlesDisinfected();
        await System.Threading.Tasks.Task.Delay(1000);
        CompleteTask();
        base.FinishTask();
    }

    protected override void OnTaskComplete() {
    }
    #endregion
}
