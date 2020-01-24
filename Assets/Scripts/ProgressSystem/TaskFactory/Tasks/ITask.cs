/// <summary>
/// Interface for Tasks.
/// </summary>
public interface ITask {
    TaskType GetTaskType();
    void SetPackage(Package package);
    void StartTask();
    /// <summary>
    /// Checks for task completion. Determines will the task be finished or moved.
    /// </summary>
    void CompleteTask();
    /// <summary>
    /// Purely finishes the task, nothing more.
    /// </summary>
    void FinishTask();
    string GetDescription();
    int GetPoints();
    string GetHint();
    void Subscribe();
    void SubscribeEvent(Events.EventDataCallback action, EventType Event);
    void UnsubscribeAllEvents();
    void RemoveFromPackage();
}
