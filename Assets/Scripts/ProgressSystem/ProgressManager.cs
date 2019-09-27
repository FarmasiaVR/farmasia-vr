using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager {

    #region fields
    private bool tasksFinished = false;
    private bool testMode;
    public List<ITask> ActiveTasks { get; private set; }
    public List<TaskType> DoneTypes { get; private set; }
    public ScoreCalculator Calculator { get; private set; }
    private float waitTime = 5.0f;
    #endregion

    #region initialization
    /// <summary>
    /// Initiates ProgressManager fields.
    /// </summary>
    public ProgressManager(bool testMode) {
        this.testMode = testMode;
        ActiveTasks = new List<ITask>();
        DoneTypes = new List<TaskType>();
        Calculator = new ScoreCalculator();
        AddTasks();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Creates a single task from every enum TaskType object.
    /// Adds tasks into currently activeTasks.
    /// </summary>
    private void AddTasks() {
        ActiveTasks = Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Select(v => TaskFactory.GetTask(v))
            .Where(v => v != null)
            .ToList();
        foreach (ITask task in ActiveTasks) {
            task.SetReferredManager(this);
        }
        ChangeDescription();
    }

    /// <summary>
    /// Once all tasks are finished, FinishProgress is called to create a Congratulation popup.
    /// </summary>
    private void FinishProgress() {
        Finish fin = new Finish();
        fin.SetReferredManager(this);
        ActiveTasks.Add(new Finish());
        if (!testMode) {
            ActiveTasks.Last().FinishTask();
        }
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Used for settings new tasks after certain points, for example player 
    /// </summary>
    /// <param name="newTask"></param>
    /// <param name="previousTask"></param>
    public void AddNewTaskBeforeTask(ITask newTask, ITask previousTask) {
        newTask.SetReferredManager(this);
        ActiveTasks.Insert(ActiveTasks.IndexOf(previousTask), newTask);
    }



    /// <summary>
    /// Adds a task to the current active list.
    /// </summary>
    /// <param name="task">Refers to task to be added.</param>
    public void AddTask(ITask task) {
        task.SetReferredManager(this);
        ActiveTasks.Add(task);
    }


    public void ListActiveTasks() {
        foreach (ITask task in ActiveTasks) {
            Logger.Print(task.GetType());
        }
    }

    private void ChangeDescription() {
        if (!testMode) {
            UISystem.Instance.ChangeDescription(ActiveTasks.First().GetDescription(), new Color(255, 255, 255, 1.0f));
        }
    }

    public void ResetTasks(bool init) {
        tasksFinished = false;
        ActiveTasks = new List<ITask>();
        DoneTypes = new List<TaskType>();
        Calculator = new ScoreCalculator();
        if (init) {
            AddTasks();
        }
    }

    public void TestPrint(string text) {
        Logger.Print(text);
    }


    /// <summary>
    /// Removes task from current active list and adds them to doneTasks list.
    /// Task is closed once it is removed from the active list.
    /// </summary>
    /// <param name="task">Refers to task to be removed.</param>
    public void RemoveTask(ITask task) {
        DoneTypes.Add(task.GetTaskType());
        ActiveTasks.Remove(task);
        ChangeDescription();
        if (!tasksFinished) {
            if (ActiveTasks.Count == 0) {
                tasksFinished = true;
                if (testMode) {
                    FinishProgress();
                } else {
                    G.Instance.Pipeline.New().Delay(waitTime).Func(FinishProgress);
                }
            } else {
                Debug.Log("Still " + ActiveTasks.Count + " left!");
            }
        }
    }
    #endregion
}
