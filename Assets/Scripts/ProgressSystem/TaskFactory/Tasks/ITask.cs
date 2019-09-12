public interface ITask {

    void FinishTask();
    string GetDescription();
    string GetHint();
    bool CheckPreviousTaskCompletion();

    bool CheckClearConditions();

    void Subscribe();

    void SubscribeEvent(Events.EventDataCallback action, EventType Event);
    void UnsubscribeAllEvents();
}
