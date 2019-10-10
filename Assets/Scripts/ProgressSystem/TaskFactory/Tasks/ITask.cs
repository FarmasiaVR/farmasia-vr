/// <summary>
/// Interface for Tasks.
/// </summary>
public interface ITask {
    TaskType GetTaskType();
    void SetReferredManager(ProgressManager manager);
    void FinishTask();
    string GetDescription();
    string GetHint();
    void Subscribe();
    void SubscribeEvent(Events.EventDataCallback action, EventType Event);
    void UnsubscribeAllEvents();
}
