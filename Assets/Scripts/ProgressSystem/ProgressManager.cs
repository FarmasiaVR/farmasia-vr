using System;
using System.Linq;
using System.Collections.Generic;

public class ProgressManager {

    #region Fields
    private bool testMode;

    // Actual list of every task. No task is ever removed from this list
    private List<Task> trueAllTasksThatAreNeverRemoved;
    private Dictionary<TaskType, int> taskMaxPoints;

    // Oot actually all tasks
    private HashSet<Task> allTasks;
    public List<Package> packages;
    public Package CurrentPackage {
        get; private set;
    }
    public ScoreCalculator Calculator {
        get; private set;
    }


    #endregion

    public ProgressManager(bool testMode) {
        this.testMode = testMode;
        allTasks = new HashSet<Task>();
        packages = new List<Package>();
        trueAllTasksThatAreNeverRemoved = new List<Task>();
        taskMaxPoints = new Dictionary<TaskType, int>();
    }

    public void ForceCloseTasks(Task calledTask) {

        Logger.Print("Total task count " + trueAllTasksThatAreNeverRemoved.Count.ToString());

        foreach (Task task in trueAllTasksThatAreNeverRemoved) {
            if (calledTask.TaskType == task.TaskType) {
                continue;
            }
            if (task.TaskType == TaskType.Finish || task.TaskType == TaskType.ScenarioOneCleanUp) {
                continue;
            }
            Logger.Print(string.Format(
                "max points: {0}, points: {1}",
                task.TaskType.ToString(),
                taskMaxPoints[task.TaskType].ToString()
            ));
            task.ForceClose(taskMaxPoints[task.TaskType] > 0);
        }
    }

    public void ForceCloseTask(TaskType type, bool killPoints = true) {
        foreach (Task task in trueAllTasksThatAreNeverRemoved) {
            if (task.TaskType == type && !task.Completed) {
                if (killPoints) {
                    task.ForceClose(taskMaxPoints[type] > 0);
                } else {
                    task.ForceClose(false);
                }

                return;
            }
        }
    }

    public void SetSceneType(SceneTypes scene) {


        switch (scene) {
            case SceneTypes.MainMenu:
                return;
            case SceneTypes.MedicinePreparation:
                /*Need Support for multiple Scenarios.*/
                GenerateScenarioOne();
                Calculator = new ScoreCalculator(trueAllTasksThatAreNeverRemoved);
                break;
            case SceneTypes.MembraneFilteration:
                GenerateScenarioTwo();
                Calculator = new ScoreCalculator(trueAllTasksThatAreNeverRemoved);
                break;
            case SceneTypes.Tutorial:
                return;
            case SceneTypes.ChangingRoom:
                return;
        }
        if (scene != SceneTypes.MainMenu) {

            CurrentPackage = packages.First();
            UpdateDescription();
            UpdateHint();
            CurrentPackage.StartTask();
        }
    }

    public void SetProgress(byte[] state) {
        ScoreCalculator c = DataSerializer.Deserializer<ScoreCalculator>(state);
        if (c != null) {
            Calculator = c;
        }
    }

    #region Initialization
    /// <summary>
    /// Used to generate every package. Package is defined with a list of tasks.
    /// </summary>
    private void GenerateScenarioOne() {
        TaskType[] selectTasks = {
            TaskType.SelectTools,
            TaskType.SelectMedicine,
            TaskType.CorrectItemsInThroughput
        };
        TaskType[] workSpaceTasks = {
            TaskType.CorrectItemsInLaminarCabinet,
            TaskType.MedicineToSyringe,
            TaskType.LuerlockAttach,
            TaskType.SyringeAttach,
            TaskType.CorrectAmountOfMedicineSelected,
            TaskType.ItemsToSterileBag
        };
        TaskType[] cleanUpTasks = {
            TaskType.ScenarioOneCleanUp,
            TaskType.Finish
        };

        packages.Add(CreatePackage(PackageName.EquipmentSelection, new List<TaskType>(selectTasks)));
        packages.Add(CreatePackage(PackageName.Workspace, new List<TaskType>(workSpaceTasks)));
        packages.Add(CreatePackage(PackageName.CleanUp, new List<TaskType>(cleanUpTasks)));
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
            TaskType.CloseAgarplates,
            TaskType.WriteSecondTime,
            TaskType.Fingerprints
        };
        TaskType[] cleanUpTasks = {
            TaskType.FinishMembrane
        };

        packages.Add(CreatePackage(PackageName.EquipmentSelection, new List<TaskType>(selectTasks)));
        packages.Add(CreatePackage(PackageName.Workspace, new List<TaskType>(workSpaceTasks)));
        packages.Add(CreatePackage(PackageName.CleanUp, new List<TaskType>(cleanUpTasks)));
    }
    
    private Package CreatePackage(PackageName name, List<TaskType> tasks) {
        Package package = new Package(name, this);
        foreach (TaskType type in tasks) {
            Task task = TaskFactory.GetTask(type);
            package.AddTask(task);

            trueAllTasksThatAreNeverRemoved.Add(task);
            taskMaxPoints.Add(task.TaskType, task.Points);
        }
        return package;
    }

    public bool IsTaskCompleted(TaskType task) {
        if (CurrentPackage.doneTypes.Contains(task)) {
            return true;
        }
        return false;
    }

    #endregion

    #region Task Movement

    /// <summary>
    /// Finds task with given type that is started and not completed
    /// </summary>
    /// <param name="taskType">Type of task to find.</param>
    /// <returns></returns>
    public Task FindTaskWithType(TaskType taskType) {
        Task foundTask = null;
        foreach (Task task in trueAllTasksThatAreNeverRemoved) {
            if (task.Completed) continue;
            if (!task.Started) continue;
            if (task.TaskType == taskType) {
                foundTask = task;
                break;
            }
        }
        return foundTask;
    }
    #endregion


    public HashSet<Task> GetAllTasks() {
        return allTasks;
    }


    #region Finishing Packages and Manager

    public void ChangePackage() {
        int index = packages.IndexOf(CurrentPackage);
        if ((index + 1) >= packages.Count) {
            FinishProgress();
        } else {
            CurrentPackage = packages[index + 1];
            CurrentPackage.StartTask();
            UpdateHint();
        }
    }

    public void FinishProgress() {
        foreach (Task task in allTasks) {
            if (task.TaskType == TaskType.Finish) {
                RemoveTask(task);
                MedicinePreparationScene.SavedScoreState = null;
                break;
            }
            if (task.TaskType == TaskType.FinishMembrane) {
                RemoveTask(task);
                MembraneFilterationScene.SavedScoreState = null;
                break;
            }
        }
        (int score, string scoreString) = Calculator.GetScoreString();
        EndSummary.EnableEndSummary(scoreString);
        Player.SavePlayerData(score, scoreString);
    }

    /// <summary>
    /// Called when task is finished and set to remove itself.
    /// </summary>
    /// <param name="task">Reference to the task that will be removed.</param>
    public void RemoveTask(Task task) {
        if (allTasks.Contains(task)) {
            allTasks.Remove(task);
        }
    }
    #endregion

    #region Description Methods
    public void UpdateDescription() {
        if (!testMode) {
            if (CurrentPackage != null && CurrentPackage.CurrentTask != null) {
                UISystem.Instance.Descript = CurrentPackage.CurrentTask.Description;
                #if UNITY_NONVRCOMPUTER
                #else
                VRVibrationManager.Vibrate();
                #endif
            } else {
                UISystem.Instance.Descript = "";
            }
        }
    }
    #endregion

    #region Hint Methods
    public void UpdateHint() {
        if (!testMode && CurrentPackage != null && CurrentPackage.CurrentTask != null) {
            HintBox.CreateHint(CurrentPackage.activeTasks[0].Hint);
        }
    }
    #endregion
}