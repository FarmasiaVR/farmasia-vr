using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager {

    #region fields
    public List<ITask> ActiveTasks { get; private set; }
    public List<TaskType> DoneTypes { get; private set; }
    public ScoreCalculator Calculator { get; }
    private float waitTime = 5.0f;
    #endregion

    #region initialization
    /// <summary>
    /// Initiates ProgressManager fields.
    /// </summary>
    public ProgressManager() {
        ActiveTasks = new List<ITask>();
        DoneTypes = new List<TaskType>();
        Calculator = new ScoreCalculator();
        AddTasks();
        UISystem.Instance.ChangeDescription(ActiveTasks.First().GetDescription(), new Color(255, 255, 255, 1.0f));
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
    }

    /// <summary>
    /// Once all tasks are finished, FinishProgress is called to create a Congratulation popup.
    /// </summary>
    private void FinishProgress() {
        foreach (ITask task in ActiveTasks) {
            task.FinishTask();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds a task to the current active list.
    /// </summary>
    /// <param name="task">Refers to task to be added.</param>
    public void AddTask(ITask task) {
        ActiveTasks.Add(task);
    }

    /// <summary>
    /// Removes task from current active list and adds them to doneTasks list.
    /// Tasks are still active inside doneTasks list!
    /// </summary>
    /// <param name="task">Refers to task to be removed.</param>
    public void RemoveTask(ITask task) {
        DoneTypes.Add(task.GetTaskType());
        ActiveTasks.Remove(task);
        if (ActiveTasks.Count == 1) {
            G.Instance.Pipeline.New().Delay(waitTime).Func(FinishProgress);
        } else {
            Debug.Log("Still " + ActiveTasks.Count + " left!");
        }
    }
    #endregion
}
