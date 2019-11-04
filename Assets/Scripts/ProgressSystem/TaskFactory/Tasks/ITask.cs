/// <summary>
/// Interface for Tasks.
/// </summary>
public interface ITask {
    TaskType GetTaskType();
    void SetPackage(Package package);
    void FinishTask();
    string GetDescription();
    int GetPoints();
    string GetHint();
    void Subscribe();
    void SubscribeEvent(Events.EventDataCallback action, EventType Event);
    void UnsubscribeAllEvents();
    void RemoveFromPackage();
}
