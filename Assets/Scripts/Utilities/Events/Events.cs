using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to subscribe to certain events with a callback
/// </summary>
public static class Events {

    #region fields
    public delegate void EventCallback();
    public delegate void EventCallbackWithData(CallbackData data);

    public static Dictionary<EventType, EventCallbackWithData> events;
    private static Dictionary<EventType, ComponentEventsContainer> componentEvents;
    #endregion

    static Events() {
        Reset();
    }

    public static void Reset() {
        events = new Dictionary<EventType, EventCallbackWithData>();
        componentEvents = new Dictionary<EventType, ComponentEventsContainer>();
    }

    #region Subscribe

    /// <summary>
    /// Subscribe to an event with a callback
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void SubscribeToEvent(EventCallback callback, EventType type) {
        void EncapsulateCallback(CallbackData data) {
            callback();
        }
        SubscribeToEvent(EncapsulateCallback, type);
    }
    public static void SubscribeToEvent(EventCallbackWithData callback, EventType type) {

        if (!events.ContainsKey(type)) {
            events.Add(type, callback);
        } else {
            events[type] += callback;
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback from a component. The subscription is removed automatically when the component is destroyed.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="component"></param>
    /// <param name="type"></param>
    public static void SubscribeToEvent(EventCallback callback, Component component, EventType type) {
        void EncapsulateCallback(CallbackData data) {
            callback();
        }
        SubscribeToEvent(EncapsulateCallback, component, type);
    }
    public static void SubscribeToEvent(EventCallbackWithData callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (!componentEvents.ContainsKey(type)) {
            ComponentEventsContainer cont = new ComponentEventsContainer();
            cont.AddPair(pair);
            componentEvents.Add(type, cont);
        } else {
            componentEvents[type].AddPair(pair);
        }
    }
    #endregion

    #region Override subscribe
    /// <summary>
    /// Subscribe to an event with a callback. This removes all previous subscriptions for this event.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void OverrideSubscription(EventCallback callback, EventType type) {
        void EncapsulateCallback(CallbackData data) {
            callback();
        }
        OverrideSubscription(EncapsulateCallback, type);
    }
    public static void OverrideSubscription(EventCallbackWithData callback, EventType type) {

        if (!events.ContainsKey(type)) {
            events.Add(type, callback);
        } else {
            events[type] = callback;
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback. This removes all previous subscriptions for this event.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void OverrideSubscription(EventCallback callback, Component component, EventType type) {
        void EncapsulateCallback(CallbackData data) {
            callback();
        }
        OverrideSubscription(EncapsulateCallback, component, type);
    }
    public static void OverrideSubscription(EventCallbackWithData callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        ComponentEventsContainer cont = new ComponentEventsContainer();
        cont.AddPair(pair);
        componentEvents.Add(type, cont);
    }
    #endregion

    #region Unsubscribe
    /// <summary>
    /// Unsubscribes a callback from an event
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void UnsubscribeFromEvent(EventCallback callback, EventType type) {
        void EncapsulateCallback(CallbackData data) {
            callback();
        }
        UnsubscribeFromEvent(EncapsulateCallback, type);
    }
    public static void UnsubscribeFromEvent(EventCallbackWithData callback, EventType type) {
        if (events.ContainsKey(type)) {
            events[type] -= callback;
        }
    }
    public static void UnsubscribeFromEvent(EventCallback callback, Component component, EventType type) {
        void EncapsulateCallback(CallbackData data) {
            callback();
        }
        UnsubscribeFromEvent(EncapsulateCallback, component, type);
    }
    public static void UnsubscribeFromEvent(EventCallbackWithData callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (componentEvents.ContainsKey(type)) {
            componentEvents[type].Unsubscribe(pair);
        }
    }
    #endregion

    #region Call callbacks

    /// <summary>
    /// Calls all callbacks for event
    /// </summary>
    /// <param name="type"></param>
    public static void FireEvent(EventType type) {
        CallDefaultCallbacks(type, CallbackData.NoData());
        CallComponentCallbacks(type, CallbackData.NoData());
    }
    public static void FireEvent(EventType type, CallbackData data) {
        CallDefaultCallbacks(type, data);
        CallComponentCallbacks(type, data);
    }

    private static void CallDefaultCallbacks(EventType type, CallbackData data) {
        events[type]?.Invoke(data);
    }

    private static void CallComponentCallbacks(EventType type, CallbackData data) {

        ComponentEventsContainer cont;

        componentEvents.TryGetValue(type, out cont);
        cont?.FireIfNotNull(data);
    }
    #endregion
}