using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour {
    #region Fields
    public static ProgressManager Instance { get; private set; }
    [SerializeField]
    private List<ITask> activeTasks;
    private List<TaskType> doneTypes;
    private List<ITask> doneTasks;
    private ScoreCalculator calculator;
    #endregion

    #region Initialize
    /// <summary>
    /// Creates a singleton of ProgressManager.
    /// </summary>
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    /// <summary>
    /// Initiates ProgressManager fields.
    /// </summary>
    private void Start() {
        activeTasks = new List<ITask>();
        doneTasks = new List<ITask>();
        doneTypes = new List<TaskType>();
        calculator = new ScoreCalculator();
        AddTasks();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Creates a single task from every enum TaskType object.
    /// Adds tasks into currently activeTasks.
    /// </summary>
    private void AddTasks() {
        activeTasks = Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Where(v => !v.Equals(TaskType.MissingItems))
            .Select(v => TaskFactory.GetTask(v))
            .ToList();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds task to the current active list.
    /// </summary>
    /// <param name="task">Refers to task to be added.</param>
    public void AddTask(ITask task) {
        activeTasks.Add(task); 
    }
    /// <summary>
    /// Removes task from current active list and adds them to doneTasks list.
    /// Tasks are still active inside doneTasks list!
    /// </summary>
    /// <param name="task">Refers to task to be removed.</param>
    public void RemoveTask(ITask task) {
        doneTasks.Add(task);
        doneTypes.Add(task.GetTaskType());
        activeTasks.Remove(task);  
    }
    /// <summary>
    /// Returns list presentation of active tasks.
    /// </summary>
    /// <returns>returns activeTasks list.</returns>
    public List<ITask> GetActiveTasks() {
        return activeTasks;
    }

    /// <summary>
    /// Returns list presentation of completed tasks.
    /// </summary>
    /// <returns>returns doneTypes list.</returns>
    public List<TaskType> GetDoneTaskTypes() {
        return doneTypes;
    }

    /// <summary>
    /// Returns Score Calculator for point addition.
    /// </summary>
    /// <returns>Returns ScoreCalculator object</returns>
    public ScoreCalculator GetCalculator() {
        return calculator;
    }
    #endregion
}
