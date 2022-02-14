using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package {

    private ProgressManager manager;
    public PackageName name { get; private set; }
    public bool packageCompleted { get; private set; }
    public List<ITask> activeTasks { get; private set; }
    public List<TaskType> doneTypes { get; private set; }

    public ITask CurrentTask {
        get {
            if (activeTasks.Count == 0) {
                return null;
            }
            return activeTasks[0];
        }
    }

    /// <summary>
    /// Initiates all fields.
    /// </summary>
    /// <param name="name">Name of the package.</param>
    /// <param name="manager">Reference to manager that created this package.</param>
    public Package(PackageName name, ProgressManager manager) {
        this.name = name;
        this.manager = manager;
        packageCompleted = false;
        activeTasks = new List<ITask>();
        doneTypes = new List<TaskType>();
    }

    /// <summary>
    /// Adds a task and updates description.
    /// </summary>
    /// <param name="task">Given task to add.</param>
    public void AddTask(ITask task) {
        task.SetPackage(this);
        activeTasks.Add(task);
        manager.UpdateDescription();
    }

    #region Task Removal
    /// <summary>
    /// Removes given task, when empty changes package in manager.
    /// </summary>
    /// <param name="task">Reference to given task.</param>
    public void RemoveTask(ITask task) {
        if (activeTasks.Contains(task)) {
            doneTypes.Add(task.GetTaskType());
            activeTasks.Remove(task);
            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
            CheckChangePackage();
            manager.UpdateDescription();
            StartTask();
        }
    }
    #endregion

    private void CheckChangePackage() {
        if (activeTasks.Count == 0) {
            packageCompleted = true;
            manager.ChangePackage();
        } else {
            Debug.Log(string.Format("Still {0} left in package: {1}", activeTasks.Count.ToString(), name.ToString()));
        }
    }

    /// <summary>
    /// Moves task back to ProgressManager
    /// </summary>
    /// <param name="task">Reference to the task that will be moved</param>
    public void MoveTaskToManager(ITask task) {
        if (activeTasks.Contains(task)) {
            Logger.Print("Moving task back to manager.");
            manager.AddTask(task);
            activeTasks.Remove(task);
            CheckChangePackage();
            manager.UpdateDescription();
            StartTask();
        }
    }


    public void PrintAllTasks() {

        Logger.Print(string.Format("Package name: {0}\nThese tasks are inside currently\n", name.ToString()));
        foreach (ITask task in activeTasks) {
            Logger.Print(task.GetType());
        }
    }

    public void StartTask() {
        if (CurrentTask != null) {
            CurrentTask.StartTask();
        }
    }

}