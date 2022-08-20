using System.Collections.Generic;
using System.Linq;
using System;

public class ProgressManager {

    private List<Package> scenarioPackages;
    private List<Task> scenarioTasks;

    public ScoreCalculator Calculator { get; private set; }
    public Package CurrentPackage { get; private set; }

    public ProgressManager() {
        scenarioPackages = new List<Package>();
        scenarioTasks = new List<Task>();
    }

    public void SetSceneType(SceneTypes scene) {
        if (scene == SceneTypes.MedicinePreparation) {
            GenerateScenarioOne();
        } else if (scene == SceneTypes.MembraneFilteration) {
            GenerateScenarioTwo();
        } else if (scene == SceneTypes.ChangingRoom) {
            GenerateScenarioThree();
        } else {
            return;
        }
        Calculator = new ScoreCalculator(scenarioTasks);
        CurrentPackage = scenarioPackages.First();
        CurrentPackage.StartTask();
        UpdateDescription();
        UpdateHint();
    }

    private void GenerateScenarioOne() {
        TaskType[] selectTasks = {
            TaskType.CorrectItemsInThroughputMedicine
        };
        TaskType[] workSpaceTasks = {
            TaskType.CorrectItemsInLaminarCabinetMedicine,
            TaskType.MedicineToSyringe,
            TaskType.LuerlockAttach,
            TaskType.SyringeAttach,
            TaskType.CorrectAmountOfMedicineTransferred,
            TaskType.AllSyringesDone,
            TaskType.ItemsToSterileBag
        };
        TaskType[] cleanUpTasks = {
            TaskType.CleanTrashMedicine,
            TaskType.CorrectItemsInBasketMedicine,
            TaskType.CleanLaminarCabinetMedicine,
            TaskType.FinishMedicine
        };
        if (MainMenuFunctions.startFromBeginning) {
            scenarioPackages.Add(CreatePackage(PackageName.EquipmentSelection, new List<TaskType>(selectTasks)));
        }
        scenarioPackages.Add(CreatePackage(PackageName.Workspace, new List<TaskType>(workSpaceTasks)));
        scenarioPackages.Add(CreatePackage(PackageName.CleanUp, new List<TaskType>(cleanUpTasks)));
    }

    private void GenerateScenarioTwo() {
        TaskType[] selectTasks = {
            TaskType.CorrectItemsInThroughputMembrane
        };
        TaskType[] workSpaceTasks = {
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            TaskType.WriteTextsToItems,
            TaskType.OpenAgarplates,
            TaskType.FillBottles,
            TaskType.AssemblePump,
            TaskType.WetFilter,
            TaskType.StartPump,
            TaskType.MedicineToFilter,
            TaskType.StartPumpAgain,
            TaskType.CutFilter,
            TaskType.FilterHalvesToBottles,
            TaskType.CloseSettlePlates,
            TaskType.WriteSecondTime,
            TaskType.Fingerprints,
            TaskType.CloseFingertipPlates
        };
        TaskType[] cleanUpTasks = {
            TaskType.CloseBottles,
            TaskType.CleanTrashMembrane,
            TaskType.CorrectItemsInBasketMembrane,
            // TaskType.EmptyLaminarCabinetMembrane,
            TaskType.CleanLaminarCabinetMembrane,
            TaskType.FinishMembrane
        };
        if (MainMenuFunctions.startFromBeginning) {
            scenarioPackages.Add(CreatePackage(PackageName.EquipmentSelection, new List<TaskType>(selectTasks)));
        }
        scenarioPackages.Add(CreatePackage(PackageName.Workspace, new List<TaskType>(workSpaceTasks)));
        scenarioPackages.Add(CreatePackage(PackageName.CleanUp, new List<TaskType>(cleanUpTasks)));
    }

    private void GenerateScenarioThree() {
        TaskType[] changingRoomTasks = {
            TaskType.WearShoeCoversAndLabCoat,
            TaskType.WashGlasses,
            TaskType.WashHandsInChangingRoom,
            TaskType.GoToPreperationRoom
        };
        TaskType[] preperationRoomTasks = {
            TaskType.WearHeadCoverAndFaceMask,
            TaskType.WashHandsInPreperationRoom,
            TaskType.WearSleeveCoversAndProtectiveGloves
        };
        TaskType[] finishUpTasks = {
            TaskType.FinishChangingRoom
        };
        scenarioPackages.Add(CreatePackage(PackageName.ChangingRoom, new List<TaskType>(changingRoomTasks)));
        scenarioPackages.Add(CreatePackage(PackageName.PreperationRoom, new List<TaskType>(preperationRoomTasks)));
        scenarioPackages.Add(CreatePackage(PackageName.FinishUp, new List<TaskType>(finishUpTasks)));
    }

    private Package CreatePackage(PackageName name, List<TaskType> tasks) {
        Package package = new Package(name, this);
        foreach (TaskType type in tasks) {
            Task task = TaskFactory.GetTask(type);
            package.AddTask(task);
            scenarioTasks.Add(task);
        }
        return package;
    }

    public void UpdateDescription() {
        if (CurrentPackage != null && CurrentPackage.CurrentTask != null) {
            UISystem.Instance.Descript = CurrentPackage.CurrentTask.Description;
            VRVibrationManager.Vibrate();
        }
    }

    public async void UpdateHint() {
        // Temporary solution
        await System.Threading.Tasks.Task.Delay(10);
        if (CurrentPackage != null && CurrentPackage.CurrentTask != null) {
            HintBox.CreateHint(CurrentPackage.activeTasks[0].Hint);
        }
    }

    public void ChangePackage() {
        int currentPackageIndex = scenarioPackages.IndexOf(CurrentPackage);
        int nextPackageIndex = currentPackageIndex + 1;
        if (nextPackageIndex >= scenarioPackages.Count) {
            FinishScenario();
        } else {
            CurrentPackage = scenarioPackages[nextPackageIndex];
            CurrentPackage.StartTask();
            UpdateDescription();
            UpdateHint();
        }
    }

    public void FinishScenario() {
        (int score, string scoreString) = Calculator.GetScoreString();
        EndSummary.EnableEndSummary(scoreString);
        Player.SavePlayerData(score, scoreString);
    }

    public void ForceCloseActiveTasksInPackage(Task calledFrom, Package package) {
        int activeTasks = package.activeTasks.Count;
        for (int i = 0; i < activeTasks; i++) {
            Task task = package.activeTasks[0];
            if (task.TaskType == calledFrom.TaskType) {
                continue;
            }
            bool removePoints = task.Points > 0;
            task.ForceClose(removePoints);
        }
    }

    public void SetProgress(byte[] state) {
        ScoreCalculator calculator = DataSerializer.Deserializer<ScoreCalculator>(state);
        if (calculator != null) {
            Calculator = calculator;
        }
    }
}
