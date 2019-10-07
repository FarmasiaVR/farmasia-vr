using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager {

    #region fields
    private bool tasksFinished = false;
    private bool testMode;

    public List<ITask> optionalSteps { get; private set; }
    public List<ITask> activeTasks { get; private set; }
    public List<TaskType> doneTypes { get; private set; }
    public ScoreCalculator calculator { get; private set; }
    private float waitTime = 5.0f;
    #endregion

    #region initialization
    /// <summary>
    /// Initiates ProgressManager fields.
    /// </summary>
    public ProgressManager(bool testMode) {
        this.testMode = testMode;
        activeTasks = new List<ITask>();
        doneTypes = new List<TaskType>();
        calculator = new ScoreCalculator();
        AddTasks();
    }
    #endregion

    #region Start and Finish
    /// <summary>
    /// Creates a single task from every enum TaskType object.
    /// Adds tasks into currently activeTasks.
    /// </summary>
    private void AddTasks() {
        activeTasks = Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Select(v => TaskFactory.GetTask(v))
            .Where(v => v != null)
            .ToList();
        foreach (ITask task in activeTasks) {
            task.SetReferredManager(this);
        }
        UpdateDescription();
    }

    /// <summary>
    /// Once all tasks are finished, FinishProgress is called to create a Congratulation popup.
    /// </summary>
    private void FinishProgress() {
        Finish fin = new Finish();
        fin.SetReferredManager(this);
        activeTasks.Add(new Finish());
        if (!testMode) {
            activeTasks.Last().FinishTask();
        }
        UpdateDescription();
    }
    #endregion

    #region Task Addition
    /// <summary>
    /// Adds a task to the current active list.
    /// </summary>
    /// <param name="task">Refers to task to be added.</param>
    public void AddTask(ITask task) {
        task.SetReferredManager(this);
        activeTasks.Add(task);
    }

    /// <summary>
    /// Used for settings new tasks after certain points, for example player 
    /// </summary>
    /// <param name="newTask"></param>
    /// <param name="previousTask"></param>
    public void AddNewTaskBeforeTask(ITask newTask, ITask previousTask) {
        newTask.SetReferredManager(this);
        activeTasks.Insert(activeTasks.IndexOf(previousTask), newTask);
    }
    #endregion

    #region Task Methods
    public void ListActiveTasks() {
        foreach (ITask task in activeTasks) {
            Logger.Print(task.GetType());
        }
    }

    /// <summary>
    /// Removes task from current active list and adds them to doneTasks list.
    /// Task is closed once it is removed from the active list.
    /// </summary>
    /// <param name="task">Refers to task to be removed.</param>
    public void RemoveTask(ITask task) {
        doneTypes.Add(task.GetTaskType());
        activeTasks.Remove(task);
        UpdateDescription();
        if (!tasksFinished) {
            if (activeTasks.Count == 0) {
                tasksFinished = true;
                if (testMode) {
                    FinishProgress();
                } else {
                    G.Instance.Pipeline.New().Delay(waitTime).Func(FinishProgress);
                }
            } else {
                Debug.Log("Still " + activeTasks.Count + " left!");
            }
        }
    }

    public void ResetTasks(bool init) {
        tasksFinished = false;
        activeTasks = new List<ITask>();
        doneTypes = new List<TaskType>();
        calculator = new ScoreCalculator();
        if (init) {
            AddTasks();
        }
    }
    #endregion

    #region Description Methods
    private void UpdateDescription() {
        if (!testMode) {
            UISystem.Instance.UpdateDescription(activeTasks);
        }
    }
    #endregion
}
