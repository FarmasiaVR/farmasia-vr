using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Version 2 of current task.
/// </summary>
public class TaskBase : ITask {

    protected bool finished = false;
    protected bool removedWhenFinished = false;
    protected bool requiresPreviousTaskCompletion = false;

    protected Dictionary<string, bool> clearConditions = new Dictionary<string, bool>();

    Dictionary<Events.EventDataCallback, EventType> subscribedEvents = new Dictionary<Events.EventDataCallback, EventType>();
    /// <summary>
    /// Constructor for Task Base.
    /// </summary>
    /// <param name="remove">Task removed when finished from list.</param>
    /// <param name="previous">Task requires previous tasks completion linearly.</param>
    public TaskBase(bool remove, bool previous) {
        removedWhenFinished = remove;
        requiresPreviousTaskCompletion = previous;
    }
    /// <summary>
    /// Used for checking if previous tasks before current task are completed.
    /// </summary>
    /// <returns>
    /// Returns true if previous tasks are completed, otherwise false.
    /// </returns>
    public bool CheckPreviousTaskCompletion() {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Used for finishing current task. Task is either removed or preserved.
    /// </summary>
    public virtual void FinishTask() {
        UnsubscribeAllEvents();
        Remove();
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

    public bool CheckClearConditions() {
        if (!clearConditions.ContainsValue(false)) {
            FinishTask();
            return true;
        }
        return false;
    }

    public void ToggleCondition(string condition) {
        clearConditions[condition] = !clearConditions[condition];
    }

    public void AddConditions(string[] conditions) {
        foreach (string condition in conditions) {
            clearConditions.Add(condition, false);
        }
    }

    public void Remove() {
        if (removedWhenFinished) {
            ProgressManager.Instance.RemoveTask(this);
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
    /// <summary>
    /// Unsubscribes from all events inside Dictionary.
    /// </summary>
    public void UnsubscribeAllEvents() {
        foreach (Events.EventDataCallback action in subscribedEvents.Keys) {
            Events.UnsubscribeFromEvent(action, subscribedEvents[action]);
        }

    }
}
