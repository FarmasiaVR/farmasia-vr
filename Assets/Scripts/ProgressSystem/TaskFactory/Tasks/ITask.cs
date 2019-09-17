using System;
using System.Collections;
using System.Collections.Generic;

public interface ITask { 

    TaskType GetTaskType();
    void FinishTask();
    string GetDescription();
    string GetHint();
    void Subscribe();
    void SubscribeEvent(Events.EventDataCallback action, EventType Event);
    void UnsubscribeAllEvents();
}
