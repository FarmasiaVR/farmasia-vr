using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Inherited by every task. 
/// Handles everything related to the given task and also has useful methods.
/// </summary>
public abstract class TaskBase : ITask {

    #region Fields
    protected bool completed = false;
    protected int points = 0;
    protected Package package;
    private bool InPackage => (package != null);
    private bool started = false;
    public bool Started { get => started; }
    protected TaskType taskType;
    protected bool checkAllClearConditions = true;
    protected bool eventsUnsubscribed = true;
    protected bool isFinished = false;
    protected bool removeWhenFinished = false;
    protected bool requiresPreviousTaskCompletion = false;
    protected bool previousTasksCompleted = false;
    protected bool unsubscribeAllEvents = true;
    protected Dictionary<int, bool> clearConditions = new Dictionary<int, bool>();
    protected Dictionary<Events.EventDataCallback, EventType> subscribedEvents = new Dictionary<Events.EventDataCallback, EventType>();
    #endregion

    public TaskBase(TaskType type, bool remove, bool previous) {
        taskType = type;
        removeWhenFinished = remove;
        requiresPreviousTaskCompletion = previous;

    }

    #region Task Progression
    public void ForceClose() {
        G.Instance.Progress.Calculator.SetScoreToZero(taskType);
        G.Instance.Progress.AddMistake("Tehtävää ei suoritettu!", 2);
        CloseTask();
    }

    public virtual void StartTask() {
        started = true;
    }

    public virtual void CompleteTask() {
        completed = CheckClearConditions();
        if (completed) {
            CloseTask();
        }
    }

    private void CloseTask() {
        RemoveFromPackage();
        OnTaskComplete();
        if (unsubscribeAllEvents) {
            UnsubscribeAllEvents();
        }
    }

    protected abstract void OnTaskComplete();

    public void ForceComplete() {

    }

    public virtual void FinishTask() {
        UnsubscribeAllEvents();
        isFinished = true;
    }

    public virtual void RemoveFromPackage() {
        if (package != null) {
            if (removeWhenFinished) {
                package.RemoveTask((ITask)this);
                if (unsubscribeAllEvents) {
                    UnsubscribeAllEvents();
                }
            } else {
                package.MoveTaskToManager((ITask)this);
            }
        }
    }
    #endregion

    #region Getters
    public bool IsCompleted() => completed;
    public virtual string GetDescription() {
        return "No Description";
    }

    public virtual string GetHint() {
        return "No Hints";
    }

    public TaskType GetTaskType() {
        return taskType;
    }

    public int GetPoints() {
        return points;
    }
    #endregion

    #region Setters
    public void SetPackage(Package package) {
        this.package = package;
    }

    protected void SetCheckAll(bool value) {
        checkAllClearConditions = value;
    }
    #endregion

    #region Subscribtion
    public virtual void Subscribe() {
    }

    public void SubscribeEvent(Events.EventDataCallback action, EventType Event) {
        Events.SubscribeToEvent(action, Event);
        eventsUnsubscribed = false;
        subscribedEvents.Add(action, Event);
    }

    public void UnsubscribeEvent(Events.EventDataCallback action, EventType type) {
        Events.UnsubscribeFromEvent(action, type);
        subscribedEvents.Remove(action);
    }

    public void UnsubscribeAllEvents() {
        foreach (Events.EventDataCallback action in subscribedEvents.Keys) {
            Events.UnsubscribeFromEvent(action, subscribedEvents[action]);
        }
        eventsUnsubscribed = true;
    }
    #endregion

    #region Condition Methods
    public void EnableCondition(Enum condition) {
        if (clearConditions.ContainsKey(condition.GetHashCode())) {
            clearConditions[condition.GetHashCode()] = true;
        }
    }

    public void DisableConditions() {
        foreach (int condition in clearConditions.Keys.ToList()) {
            clearConditions[condition] = false;
        }
    }

    public void AddConditions(int[] conditions) {
        foreach (int condition in conditions) {
            clearConditions.Add(condition, false);
        }
    }

    // Check every Condition.
    private bool CheckClearConditions() {
        if (clearConditions.Values.Count == 0) {
            return true;
        }

        if (checkAllClearConditions) {
            if (!clearConditions.ContainsValue(false)) {
                return true;
            }
            return false;
        }
        if (clearConditions.ContainsValue(true)) {
            return true;
        }
        return false;
    }


    //Check for certain conditions.
    protected bool CheckClearConditions(List<int> conditions) {
        foreach (int condition in conditions) {
            if (clearConditions[condition] == false) {
                return false;
            }
        }
        return true;
    }

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

    #region Task Specific Methods
    protected bool IsNotStarted() {
        return !started;
    }

    protected bool CheckIsCurrent() {
        return (package?.activeTasks.IndexOf((ITask)this) ?? 0) == 0;
    }

    protected bool IsPreviousTasksCompleted(List<TaskType> tasks) {
        List<TaskType> completed = package.doneTypes;
        foreach (TaskType type in tasks) {
            if (!completed.Contains(type)) {
                return false;
            }
        }
        previousTasksCompleted = true;
        return true;
    }

    protected void Popup(string message, MsgType type, int point = int.MaxValue) {
        if (point != int.MaxValue) {
            G.Instance.Progress.AddTaskMistake(message);
            UISystem.Instance.CreatePopup(point, message, type);
        } else {
            UISystem.Instance.CreatePopup(message, type);
        }

        switch (type) {
            case MsgType.Mistake:
                G.Instance.Audio.Play(AudioClipType.MistakeMessage);
                break;
            case MsgType.Notify:
                G.Instance.Audio.Play(AudioClipType.MistakeMessage);
                break;
        }
    }

    public override string ToString() {

        string s = "TaskType: " + taskType + ", finished: " + isFinished;

        s += "\nstrted: " + Started;
        s += "\ncheck cond: " + checkAllClearConditions;
        s += "\nis completed: " + completed;
        s += "\nremove when finished: " + removeWhenFinished;
        s += "\nrequires previous: " + requiresPreviousTaskCompletion;
        s += "\nprevious completed: " + previousTasksCompleted;

        return s;
    }
    #endregion
}
