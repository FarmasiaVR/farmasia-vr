using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to subscribe to certain events with a callback
/// </summary>
public static class Events {

    #region fields
    public delegate void EventEmptyCallback();
    public delegate void EventDataCallback(CallbackData data);

    private static Dictionary<EventType, Delegate> eventsData;

    private static Dictionary<EventType, ComponentEventsContainer> componentEventsData;
    private static Dictionary<EventType, ComponentEventsContainer> componentEventsNoData;
    #endregion

    static Events() {
        Reset();
    }

    public static void Reset() {
        eventsData = new Dictionary<EventType, Delegate>();

        componentEventsNoData = new Dictionary<EventType, ComponentEventsContainer>();
        componentEventsData = new Dictionary<EventType, ComponentEventsContainer>();
    }

    #region Subscribe

    /// <summary>
    /// Subscribe to an event with a callback
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void SubscribeToEvent(EventEmptyCallback callback, EventType type) {
        AddToDelegateDictionary<EventType>(eventsData, type, callback);
    }

    public static void SubscribeToEvent(EventDataCallback callback, EventType type) {
        AddToDelegateDictionary<EventType>(eventsData, type, callback);
    }

    private static void AddToDelegateDictionary<K>(Dictionary<K, Delegate> dict, K key, Delegate value) {
        if (!dict.ContainsKey(key)) {
            dict.Add(key, value);
        } else {
            dict[key] = Delegate.Combine(dict[key], value);
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback from a component. The subscription is removed automatically when the component is destroyed.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="component"></param>
    /// <param name="type"></param>
    public static void SubscribeToEvent(EventEmptyCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (!componentEventsData.ContainsKey(type)) {
            ComponentEventsContainer cont = new ComponentEventsContainer();
            cont.AddPair(pair);
            componentEventsData.Add(type, cont);
        } else {
            componentEventsData[type].AddPair(pair);
        }
    }

    public static void SubscribeToEvent(EventDataCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (!componentEventsData.ContainsKey(type)) {
            ComponentEventsContainer cont = new ComponentEventsContainer();
            cont.AddPair(pair);
            componentEventsData.Add(type, cont);
        } else {
            componentEventsData[type].AddPair(pair);
        }
    }
    #endregion

    #region Override subscribe
    /// <summary>
    /// Subscribe to an event with a callback. This removes all previous subscriptions for this event.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void OverrideSubscription(EventEmptyCallback callback, EventType type) {
        SetValueDelegateDictionary(eventsData, type, callback);
    }

    public static void OverrideSubscription(EventDataCallback callback, EventType type) {
        SetValueDelegateDictionary(eventsData, type, callback);
    }

    private static void SetValueDelegateDictionary<K>(Dictionary<K, Delegate> dict, K key, Delegate value) {
        if (!dict.ContainsKey(key)) {
            dict.Add(key, value);
        } else {
            dict[key] = value;
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback. This removes all previous subscriptions for this event.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void OverrideSubscription(EventEmptyCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        ComponentEventsContainer cont = new ComponentEventsContainer();
        cont.AddPair(pair);
        componentEventsData.Add(type, cont);
    }

    public static void OverrideSubscription(EventDataCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        ComponentEventsContainer cont = new ComponentEventsContainer();
        cont.AddPair(pair);
        componentEventsData.Add(type, cont);
    }
    #endregion

    #region Unsubscribe
    /// <summary>
    /// Unsubscribes a callback from an event
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void UnsubscribeFromEvent(EventEmptyCallback callback, EventType type) {
        if (eventsData.ContainsKey(type)) {
            eventsData[type] = Delegate.RemoveAll(eventsData[type], callback);
        }
    }

    public static void UnsubscribeFromEvent(EventDataCallback callback, EventType type) {
        if (eventsData.ContainsKey(type)) {
            eventsData[type] = Delegate.RemoveAll(eventsData[type], callback);
        }
    }

    public static void UnsubscribeFromEvent(EventEmptyCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (componentEventsData.ContainsKey(type)) {
            componentEventsData[type].Unsubscribe(pair);
        }
    }

    public static void UnsubscribeFromEvent(EventDataCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (componentEventsData.ContainsKey(type)) {
            componentEventsData[type].Unsubscribe(pair);
        }
    }
    #endregion

    #region Call callbacks

    /// <summary>
    /// Calls all callbacks for event
    /// </summary>
    /// <param name="type"></param>
    public static void FireEvent(EventType type) {
        CallDefaultCallbacks(type);
        CallComponentCallbacks(type, CallbackData.NoData());
    }

    public static void FireEvent(EventType type, CallbackData data) {
        CallDefaultCallbacks(type, data);
        CallComponentCallbacks(type, data);
    }

    private static void CallDefaultCallbacks(EventType type, CallbackData? data = null) {
        if (!eventsData.ContainsKey(type)) {
            return;
        }

        if (data == null) {
            (eventsData[type] as EventEmptyCallback)?.Invoke();
        } else {
            (eventsData[type] as EventDataCallback)?.Invoke((CallbackData) data);
        }
    }

    private static void CallComponentCallbacks(EventType type, CallbackData data) {
        ComponentEventsContainer cont = null;

        if (componentEventsNoData.TryGetValue(type, out cont)) {
            cont.FireIfNotNull(data);
        }

        if (componentEventsData.TryGetValue(type, out cont)) {
            cont.FireIfNotNull(data);
        }
    }
    #endregion
}