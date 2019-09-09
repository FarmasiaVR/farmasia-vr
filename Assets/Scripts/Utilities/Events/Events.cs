using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to subscribe to certain events with a callback
/// </summary>
public static class Events {

    #region fields
    public delegate void EventCallback();
    public delegate void EventCallbackWithData(CallbackData data);

    public static Dictionary<EventType, EventCallbackWithData> eventsData;

    private static Dictionary<EventType, ComponentEventsContainer> componentEvents;
    #endregion

    static Events() {
        Reset();
    }

    public static void Reset() {
        eventsData = new Dictionary<EventType, EventCallbackWithData>();
        componentEvents = new Dictionary<EventType, ComponentEventsContainer>();
    }

    #region Subscribe

    /// <summary>
    /// Subscribe to an event with a callback
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    //public static void SubscribeToEvent(EventCallback callback, EventType type) {
    //    SubscribeToEvent(EncapsulateCallback(callback), type);
    //}
    public static void SubscribeToEvent(EventCallbackWithData callback, EventType type) {

        if (!eventsData.ContainsKey(type)) {
            eventsData.Add(type, callback);
        } else {
            eventsData[type] += callback;
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback from a component. The subscription is removed automatically when the component is destroyed.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="component"></param>
    /// <param name="type"></param>
    //public static void SubscribeToEvent(EventCallback callback, Component component, EventType type) {
    //    SubscribeToEvent(EncapsulateCallback(callback), component, type);
    //}
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
    //public static void OverrideSubscription(EventCallback callback, EventType type) {
    //    OverrideSubscription(EncapsulateCallback(callback), type);
    //}
    public static void OverrideSubscription(EventCallbackWithData callback, EventType type) {

        if (!eventsData.ContainsKey(type)) {
            eventsData.Add(type, callback);
        } else {
            eventsData[type] = callback;
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback. This removes all previous subscriptions for this event.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    //public static void OverrideSubscription(EventCallback callback, Component component, EventType type) {
    //    OverrideSubscription(EncapsulateCallback(callback), component, type);
    //}
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
    //public static void UnsubscribeFromEvent(EventCallback callback, EventType type) {
    //    UnsubscribeFromEvent(EncapsulateCallback(callback), type);
    //}
    public static void UnsubscribeFromEvent(EventCallbackWithData callback, EventType type) {
        if (eventsData.ContainsKey(type)) {
            eventsData[type] -= callback;
        }
    }
    public static void UnsubscribeFromEvent(EventCallback callback, Component component, EventType type) {
        UnsubscribeFromEvent(EncapsulateCallback(callback), component, type);
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

        if (!eventsData.ContainsKey(type)) {
            return;
        }

        eventsData[type]?.Invoke(data);
    }

    private static void CallComponentCallbacks(EventType type, CallbackData data) {

        ComponentEventsContainer cont;

        componentEvents.TryGetValue(type, out cont);
        cont?.FireIfNotNull(data);
    }
    #endregion

    private static EventCallbackWithData EncapsulateCallback(EventCallback callback) {
        return delegate (CallbackData data) {
            callback();
        };
    }
}