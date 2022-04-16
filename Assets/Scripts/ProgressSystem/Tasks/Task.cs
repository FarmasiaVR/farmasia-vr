using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Inherited by every task. 
/// Handles everything related to the given task and also has useful methods.
/// </summary>
public abstract class Task {

    public bool Completed = false;
    public readonly int Points = 0;
    protected Package package;
    private bool started = false;
    public bool Started { get => started; }
    public TaskType TaskType { get; protected set; }
    protected bool checkAllClearConditions = true;
    protected bool eventsUnsubscribed = true;
    protected bool isFinished = false;
    protected bool requiresPreviousTaskCompletion = false;
    protected bool previousTasksCompleted = false;
    protected bool unsubscribeAllEvents = true;
    protected Dictionary<int, bool> clearConditions = new Dictionary<int, bool>();
    protected Dictionary<Events.EventDataCallback, EventType> subscribedEvents = new Dictionary<Events.EventDataCallback, EventType>();

    protected string description;
    public virtual string Description { get => description; }
    protected string hint;
    public virtual string Hint { get => hint; }
    protected string success;
    

    public Task(TaskType type, bool previous) {
        TaskType = type;
        requiresPreviousTaskCompletion = previous;
        description = TaskConfig.For(TaskType).Description;
        hint = TaskConfig.For(type).Description;
        success = TaskConfig.For(type).Success;
        Points = TaskConfig.For(type).Points;
    }

    #region Task Progression
    public virtual void ForceClose(bool removePoints) {
        if (Completed) {
            return;
        }

        if (removePoints) {
            Logger.Print(string.Format("Task still has points left: {0}, points: {1}", TaskType.ToString(), Points.ToString()));
            G.Instance.Progress.Calculator.SetScoreToZero(TaskType);
            Task.CreateTaskMistake(TaskType, "Tehtävää ei suoritettu", 2);
        }
        // Next group: Re do entire progress manager or your pain will be immeasureable
        CloseTask();
        FinishTask();
        isFinished = true;
        Completed = true;
    }

    public virtual void StartTask() {
        Logger.Print("PROGRESS: started " + TaskType.ToString());
        started = true;
    }

    public virtual void CompleteTask() {
        Completed = CheckClearConditions();
        //Logger.Print("Clear conditions: " + completed);
        if (Completed) {
            CloseTask();
            Popup(success, MsgType.Done, Points);
        }
    }

    protected void CloseTask() {
        RemoveFromPackage();
        OnTaskComplete();
        if (unsubscribeAllEvents) {
            UnsubscribeAllEvents();
        }
    }

    protected virtual void OnTaskComplete() { 
    
    }

    public virtual void FinishTask() {
        UnsubscribeAllEvents();
        isFinished = true;
    }

    public virtual void RemoveFromPackage() {
        if (package != null) {
            package.RemoveTask((Task)this);
            if (unsubscribeAllEvents) {
                UnsubscribeAllEvents();
            }
        }
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
            // Logger.Print(this.TaskType.ToString() +  " Enabled Condition: " + condition.ToString());
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
            UISystem.Instance.CreatePopup(point, message, type);
        } else {
            UISystem.Instance.CreatePopup(message, type);
        }
    }

    public override string ToString() {
        System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

        string s = string.Format(
            "TaskType: {0}, finished: {1}",
            TaskType.ToString(),
            isFinished.ToString()
        );

        string[] strings = new string[] {
            s,
            "\nstrted: ", Started.ToString(),
            "\ncheck cond: ", checkAllClearConditions.ToString(),
            "\nis completed: ", Completed.ToString(),
            "\nrequires previous: ", requiresPreviousTaskCompletion.ToString(),
            "\nprevious completed: ", previousTasksCompleted.ToString()
        };

        for (int i = 0; i < strings.Length; i++) {
            strBuilder.Append(strings[i]);
        }
        
        return strBuilder.ToString();
    }

    #region Mistakes
    public void CreateTaskMistake(string mistake, int minus) {
        CreateTaskMistake(TaskType, mistake, minus);
    }
    public static void CreateGeneralMistake(string mistake, int minus = 1, bool showMessage = true) {
        if (showMessage) {
            UISystem.Instance.CreatePopup(-minus, mistake, MsgType.Mistake);
        }
        G.Instance.Progress.Calculator.CreateMistake(mistake, minus);
    }
    public static void CreateTaskMistake(TaskType type, string mistake, int minus) {
        if (mistake != null) {
            UISystem.Instance.CreatePopup(-minus, mistake, MsgType.Mistake);
            G.Instance.Progress.Calculator.CreateTaskMistake(type, mistake);
        }
        G.Instance.Progress.Calculator.SubtractPoints(type, minus);
    }
    #endregion
    #endregion
}
