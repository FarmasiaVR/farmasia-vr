using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events {

    #region fields
    public delegate void EventCallback();

    public static Dictionary<Event, EventCallback> events;

    public enum Event {
        PlayerDied,
        EnterGoal,
        ExitGoal,
        FirstSling,
        LevelFinished,
        StartLevel
    }
    #endregion

    static Events() {
        Reset();
    }

    public static void Reset() {
        events = new Dictionary<Event, EventCallback>();
    }

    public static void SubscribeToEvent(EventCallback callback, Event type) {

        if (!events.ContainsKey(type)) {
            events.Add(type, callback);
        } else {
            events[type] += callback;
        }
    }

    public static void OverrideSubscription(EventCallback callback, Event type) {

        if (!events.ContainsKey(type)) {
            events.Add(type, callback);
        } else {
            events[type] = callback;
        }
    }
    public static void UnsubscribeFromEvent(EventCallback callback, Event type) {
        if (events.ContainsKey(type)) {
            events[type] -= callback;
        }
    }

    public static void FireEvent(Event type) {

        if (!events.ContainsKey(type)) {
            return;
        }

        events[type]?.Invoke();
    }
}