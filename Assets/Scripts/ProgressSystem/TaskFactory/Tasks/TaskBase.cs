using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Inherited by every task. 
/// Handles everything related to the given task and also has useful methods.
/// </summary>
public class TaskBase : ITask {
    #region Fields
    protected int points;
    protected Package package;
    protected TaskType taskType;
    protected bool isFinished = false;
    protected bool removeWhenFinished = false;
    protected bool requiresPreviousTaskCompletion = false;
    protected bool previousTasksCompleted = false;
    protected Dictionary<int, bool> clearConditions = new Dictionary<int, bool>();
    protected Dictionary<Events.EventDataCallback, EventType> subscribedEvents = new Dictionary<Events.EventDataCallback, EventType>();
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor for Task Base.
    /// </summary>
    /// <param name="remove">Task removed when finished from list.</param>
    /// <param name="previous">Task requires previous tasks completion linearly.</param>
    public TaskBase(TaskType type, bool remove, bool previous) {
        points = 0;
        taskType = type;
        removeWhenFinished = remove;
        requiresPreviousTaskCompletion = previous;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Removes current task if the task has been set to be removed. Otherwise moves it back to manager.
    /// </summary>
    private void RemoveFromPackage() {
        if (package != null) {
            if (removeWhenFinished) {
                package.RemoveTask((ITask) this);
            } else {
                package.MoveTaskToManager((ITask) this);
            }
        }
    }
    #endregion

    #region Virtual Methods
    /// <summary>
    /// Used for finishing current task. Task is either removed or preserved.
    /// </summary>
    public virtual void FinishTask() {
        UnsubscribeAllEvents();
        RemoveFromPackage();
    }

    /// <summary>
    /// Used for getting task's description to show on UI.
    /// </summary>
    /// <returns>
    /// Returns string presentation of description.
    /// </returns>
    public virtual string GetDescription() {
        return "No Description";
    }

    /// <summary>
    /// Used for getting task's hint when hint trigger is triggered.
    /// </summary>
    /// <returns>
    /// Return string presentation of hint.
    /// </returns>
    public virtual string GetHint() {
        return "No Hints";
    }

    /// <summary>
    /// Used for defining custom Subscribtions per task. (Override)
    /// </summary>
    public virtual void Subscribe() {
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Used for summary at the end of the game.
    /// </summary>
    /// <returns>Integer of maximum points for current task.</returns>
    public int GetPoints() {
        return points;
    }
    public void SetPackage(Package package) {
        this.package = package;
    }
    /// <summary>
    /// Return the type of current task.
    /// </summary>
    /// <returns>returns TaskType enum.</returns>
    public TaskType GetTaskType() {
        return taskType;
    }

    /// <summary>
    /// Enables condition with given string.
    /// </summary>
    /// <param name="condition">String representation of condition.</param>
    public void EnableCondition(Enum condition) {
        if (clearConditions.ContainsKey(condition.GetHashCode())) {
            clearConditions[condition.GetHashCode()] = true;
        }
    }

    /// <summary>
    /// Disables all conditions.
    /// </summary>
    public void DisableConditions() {
        foreach (int condition in clearConditions.Keys.ToList()) {
            clearConditions[condition] = false;
        }
    }

    /// <summary>
    /// Adds integer values to conditions.
    /// </summary>
    /// <param name="conditions">List of conditions</param>
    public void AddConditions(int[] conditions) {
        foreach (int condition in conditions) {
            Logger.Print(condition);
            clearConditions.Add(condition, false);
        }
    }

    /// <summary>
    /// Subscribes to Events and adds them to a Dictionary.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="Event"></param>
    public void SubscribeEvent(Events.EventDataCallback action, EventType Event) {
        Events.SubscribeToEvent(action, Event);
        subscribedEvents.Add(action, Event);
    }

    public void UnsubscribeEvent(Events.EventDataCallback action, EventType type) {
        Events.UnsubscribeFromEvent(action, type);
        subscribedEvents.Remove(action);
    }

    /// <summary>
    /// Unsubscribes from all events inside Dictionary.
    /// </summary>
    public void UnsubscribeAllEvents() {
        foreach (Events.EventDataCallback action in subscribedEvents.Keys) {
            Events.UnsubscribeFromEvent(action, subscribedEvents[action]);
        }
    }
    #endregion

    #region Protected Methods
    /// <summary>
    /// Checks if task is currently executed in package. Aka first in it.
    /// </summary>
    /// <returns>True if is current, false if not.</returns>
    protected bool CheckIsCurrent() {
        return (package?.activeTasks.IndexOf((ITask)this) ?? 0) == 0;
    }

    /// <summary>
    /// Used for checking if certain previous tasks before current task are completed.
    /// </summary>
    /// <returns>
    /// Returns true if previous tasks are completed, otherwise false.
    /// </returns>
    protected bool CheckPreviousTaskCompletion(List<TaskType> tasks) {
        List<TaskType> completed = package.doneTypes;
        foreach (TaskType type in tasks) {
            if (!completed.Contains(type)) {
                return false;
            }
        }
        previousTasksCompleted = true;
        return true;
    }

    /// <summary>
    /// Checks if conditions have been cleared.
    /// </summary>
    /// <param name="checkAll"></param>
    /// <returns></returns>
    protected bool CheckClearConditions(bool checkAll) {
        foreach (int cond in GetNonClearedConditions()) {
            Logger.Print(cond);
        }
        if (checkAll) {
            if (!clearConditions.ContainsValue(false)) {
                FinishTask();
                return true;
            }
            return false;
        }
        if (clearConditions.ContainsValue(true)) {
            FinishTask();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns an array of the conditions that have not yet been cleared.
    /// </summary>
    /// <returns>An array of the names of the conditions (strings)</returns>
    protected List<int> GetNonClearedConditions() {
        List<int> nonCleared = new List<int>();
        foreach (KeyValuePair<int, bool> condition in clearConditions) {
            if (!condition.Value) {
                nonCleared.Add(condition.Key);
            }
        }
        return nonCleared;
    }
    #endregion
}
