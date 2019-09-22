using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour {
    #region Fields
    public static ProgressManager Instance { get; private set; }
    private List<ITask> activeTasks;
    private List<TaskType> doneTypes;
    private List<ITask> doneTasks;
    private ScoreCalculator calculator;
    private bool isFinished = false;
    private float finishTimer = 0.0f;
    private float waitTime = 5.0f;
    #endregion

    #region Initialisation
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
    /// Once all Tasks are finished, waits for a moment and then finishes progress.
    /// </summary>
    private void Update() {
        if (isFinished) {
            finishTimer += Time.deltaTime;
            if (finishTimer >= waitTime) {
                FinishProgress();
                isFinished = false;
            }
        }
    }

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
    }

    /// <summary>
    /// Once all tasks are finished, FinishProgress is called to create a Congratulation popup.
    /// </summary>
    private void FinishProgress() {
        UISystem.Instance.CreatePopup(0, "Congratulations!\nAll tasks finished", MessageType.Congratulate);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Removes task from current active list and adds them to doneTasks list.
    /// Tasks are still active inside doneTasks list!
    /// </summary>
    /// <param name="task">Refers to task to be removed.</param>
    public void RemoveTask(ITask task) {
        doneTasks.Add(task);
        doneTypes.Add(task.GetTaskType());
        activeTasks.Remove(task);
        if (activeTasks.Count == 0) {
            isFinished = true;
        } else {
            Debug.Log("Still " + activeTasks.Count + " left!");
        }

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
