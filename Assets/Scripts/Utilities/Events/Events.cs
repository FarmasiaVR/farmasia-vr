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
    public static Dictionary<EventType, EventCallback> eventsNoData;

    private static Dictionary<EventType, ComponentEventsContainer> componentEventsData;
    private static Dictionary<EventType, ComponentEventsContainer> componentEventsNoData;
    #endregion

    static Events() {
        Reset();
    }

    public static void Reset() {

        eventsNoData = new Dictionary<EventType, EventCallback>();
        eventsData = new Dictionary<EventType, EventCallbackWithData>();

        componentEventsNoData = new Dictionary<EventType, ComponentEventsContainer>();
        componentEventsData = new Dictionary<EventType, ComponentEventsContainer>();
    }

    #region Subscribe

    /// <summary>
    /// Subscribe to an event with a callback
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void SubscribeToEvent(EventCallback callback, EventType type) {

        if (!eventsNoData.ContainsKey(type)) {
            eventsNoData.Add(type, callback);
        } else {
            eventsNoData[type] += callback;
        }
    }
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
    public static void SubscribeToEvent(EventCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (!componentEventsData.ContainsKey(type)) {
            ComponentEventsContainer cont = new ComponentEventsContainer();
            cont.AddPair(pair);
            componentEventsData.Add(type, cont);
        } else {
            componentEventsData[type].AddPair(pair);
        }
    }
    public static void SubscribeToEvent(EventCallbackWithData callback, Component component, EventType type) {

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
    public static void OverrideSubscription(EventCallback callback, EventType type) {

        if (!eventsNoData.ContainsKey(type)) {
            eventsNoData.Add(type, callback);
        } else {
            eventsNoData[type] = callback;
        }
    }
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
    public static void OverrideSubscription(EventCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        ComponentEventsContainer cont = new ComponentEventsContainer();
        cont.AddPair(pair);
        componentEventsData.Add(type, cont);
    }
    public static void OverrideSubscription(EventCallbackWithData callback, Component component, EventType type) {

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
    public static void UnsubscribeFromEvent(EventCallback callback, EventType type) {

        if (eventsNoData.ContainsKey(type)) {
            eventsNoData[type] -= callback;
        }
    }
    public static void UnsubscribeFromEvent(EventCallbackWithData callback, EventType type) {
        if (eventsData.ContainsKey(type)) {
            eventsData[type] -= callback;
        }
    }
    public static void UnsubscribeFromEvent(EventCallback callback, Component component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (componentEventsData.ContainsKey(type)) {
            componentEventsData[type].Unsubscribe(pair);
        }
    }
    public static void UnsubscribeFromEvent(EventCallbackWithData callback, Component component, EventType type) {

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
        CallDefaultCallbacks(type, CallbackData.NoData());
        CallComponentCallbacks(type, CallbackData.NoData());
    }
    public static void FireEvent(EventType type, CallbackData data) {
        CallDefaultCallbacks(type, data);
        CallComponentCallbacks(type, data);
    }

    private static void CallDefaultCallbacks(EventType type, CallbackData data) {

        // No data
        if (eventsNoData.ContainsKey(type)) {
            eventsNoData[type]?.Invoke();
        }

        

        // With data
        if (eventsData.ContainsKey(type)) {
            eventsData[type]?.Invoke(data);
        }
    }

    private static void CallComponentCallbacks(EventType type, CallbackData data) {

        // No data
        ComponentEventsContainer cont;

        componentEventsNoData.TryGetValue(type, out cont);
        cont?.FireIfNotNull(data);

        // With data

        componentEventsData.TryGetValue(type, out cont);
        cont?.FireIfNotNull(data);
    }
    #endregion

    //private static EventCallbackWithData EncapsulateCallback(EventCallback callback) {
    //    return delegate (CallbackData data) {
    //        callback();
    //    };
    //}
}