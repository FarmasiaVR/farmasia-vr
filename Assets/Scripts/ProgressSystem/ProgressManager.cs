using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager {

    #region Fields
    private bool testMode;
    private HashSet<ITask> allTasks;
    public List<Package> packages;
    public Package CurrentPackage { get; private set; }
    public ScoreCalculator Calculator { get; private set; }
    private float waitTime = 5.0f;
    #endregion

    #region Constructor
    /// <summary>
    /// Initiates ProgressManager fields.
    /// </summary>
    public ProgressManager(bool testMode) {
        this.testMode = testMode;
        allTasks = new HashSet<ITask>();
        packages = new List<Package>();
        AddTasks();
        Calculator = new ScoreCalculator(allTasks);
        GenerateScenarioOne();
        CurrentPackage = packages.First();
        UpdateDescription();
        UpdateHint();
    }
    #endregion

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
            //TaskType.ScenarioOneCleanUp,
            TaskType.Finish 
        };

        Package equipmentSelection = CreatePackageWithList(PackageName.EquipmentSelection, new List<TaskType>(selectTasks));
        Package workSpace = CreatePackageWithList(PackageName.Workspace, new List<TaskType>(workSpaceTasks));
        Package cleanUp = CreatePackageWithList(PackageName.CleanUp, new List<TaskType>(cleanUpTasks));
        packages.Add(equipmentSelection);
        packages.Add(workSpace);
        packages.Add(cleanUp);
    }

    #region Package Init Functions
    /// <summary>
    /// Creates a new package.
    /// </summary>
    /// <param name="name">Name of the new package.</param>
    /// <param name="tasks">List of predefined tasks.</param>
    /// <returns></returns>
    private Package CreatePackageWithList(PackageName name, List<TaskType> tasks) {
        Package package = new Package(name, this);
        TakeTasksToPackage(package, tasks);
        return package;
    }

    /// <summary>
    /// Takes tasks from ProgressManager to designated package.
    /// </summary>
    /// <param name="package">Package to move tasks to.</param>
    /// <param name="types">List of types to move.</param>
    private void TakeTasksToPackage(Package package, List<TaskType> types) {
        foreach (TaskType type in types) {
            MoveToPackage(package, type);
        }
    }
    #endregion
    #endregion

    #region Task Addition
    /// <summary>
    /// Creates a single task from every enum TaskType object.
    /// Adds tasks into currently activeTasks.
    /// </summary>
    public void AddTasks() {
        allTasks = new HashSet<ITask>(Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Select(v => TaskFactory.GetTask(v))
            .Where(v => v != null)
            .ToList());
    }

    public void AddTask(ITask task) {
        if (!allTasks.Contains(task)) {
            allTasks.Add(task);
        }
    }
    #endregion

    #region Task Movement
    /// <summary>
    /// Moves task to package with given TaskType.
    /// </summary>
    /// <param name="package">Package to move task to.</param>
    /// <param name="taskType">Type of task to move.</param>
    public void MoveToPackage(Package package, TaskType taskType) {
        ITask foundTask = FindTaskWithType(taskType);
        if (foundTask != null) {
            package.AddTask(foundTask);
            allTasks.Remove(foundTask);
        }
    }

    /// <summary>
    /// Moves task to package into given point. Used by packages.
    /// </summary>
    /// <param name="package">Packages to move task to.</param>
    /// <param name="taskType">Type to move.</param>
    /// <param name="previousTask">Task point where found task will be moved.</param>
    public void MoveToPackageBeforeTask(Package package, TaskType taskType, ITask previousTask) {
        ITask foundTask = FindTaskWithType(taskType);
        if (foundTask != null) {
            package.AddNewTaskBeforeTask(foundTask, previousTask);
            allTasks.Remove(foundTask);
        }
    }

    /// <summary>
    /// Finds task with given type.
    /// </summary>
    /// <param name="taskType">Type of task to find.</param>
    /// <returns></returns>
    public ITask FindTaskWithType(TaskType taskType) {
        ITask foundTask = null;
        foreach (ITask task in allTasks) {
            if (task.GetTaskType() == taskType) {
                foundTask = task;
                break;
            }
        }
        return foundTask;
    }
    #endregion

    #region Task Methods

    public HashSet<ITask> GetAllTasks() {
        return allTasks;
    }
    public void ListAllTasksInManager() {
        foreach (ITask task in allTasks) {
            Logger.Print(task.GetType());
        }
    }
    #endregion

    #region Finishing Packages and Manager

    public void ChangePackage() {
        int index = packages.IndexOf(CurrentPackage);
        if (packages[index + 1] != null) {
            CurrentPackage = packages[index + 1];
        } else {
            FinishProgress();
        }
    }

    /// <summary>
    /// Calls every task inside allTasks Set and finishes them.
    /// </summary>
    public void FinishProgress() {
        foreach (ITask task in allTasks) {
            if (task.GetTaskType() == TaskType.Finish) {
               task.FinishTask(); 
               break;
            }
        }
    }

    /// <summary>
    /// Called when task is finished and set to remove itself.
    /// </summary>
    /// <param name="task">Reference to the task that will be removed.</param>
    public void RemoveTask(ITask task) {
        if (allTasks.Contains(task)) {
            allTasks.Remove(task);
        }
    }
    #endregion

    #region Description Methods
    public void UpdateDescription() {
        if (!testMode) {
            if (CurrentPackage != null) {
                UISystem.Instance.UpdateDescription(CurrentPackage.activeTasks);
            }
        }
    }
    #endregion

    #region Hint Methods
    public void UpdateHint() {
        if (!testMode) {
            if (CurrentPackage != null) {
                HintBox.CreateHint(CurrentPackage.activeTasks[0].GetHint());
            }
        }
    }
    #endregion

    public bool IsCurrentPackage(PackageName name) {
        return CurrentPackage.name == name;
    }
}