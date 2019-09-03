using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to subscribe to certain events with a callback
/// </summary>
public static class Events {

    #region fields
    public delegate void EventCallback(string data);

    public static Dictionary<EventType, EventCallback> events;
    private static Dictionary<EventType, ComponentEventsContainer> componentEvents;

    #endregion

    static Events() {
        Reset();
    }

    public static void Reset() {
        events = new Dictionary<EventType, EventCallback>();
        componentEvents = new Dictionary<EventType, ComponentEventsContainer>();


        
    }

    #region Default events

    /// <summary>
    /// Subscribe to an event with a callback
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void SubscribeToEvent(EventCallback callback, EventType type) {

        if (!events.ContainsKey(type)) {
            events.Add(type, callback);
        } else {
            events[type] += callback;
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback. This removes all previous subscriptions for this event.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void OverrideSubscription(EventCallback callback, EventType type) {

        if (!events.ContainsKey(type)) {
            events.Add(type, callback);
        } else {
            events[type] = callback;
        }
    }

    /// <summary>
    /// Unsubscribes a callback from an event
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void UnsubscribeFromEvent(EventCallback callback, EventType type) {
        if (events.ContainsKey(type)) {
            events[type] -= callback;
        }
    }

    private static void CallDefaultCallbacks(EventType type, string data) {
        events[type]?.Invoke(data);
    }
    #endregion

    #region Component events

    /// <summary>
    /// Subscribe to an event with a callback from a component. The subscription is removed automatically when the component is destroyed.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="component"></param>
    /// <param name="type"></param>
    public static void SubscribeToEvent(EventCallback callback, MonoBehaviour component, EventType type) {


        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (!componentEvents.ContainsKey(type)) {
            ComponentEventsContainer cont = new ComponentEventsContainer();
            cont.AddPair(pair);
            componentEvents.Add(type, cont);
        } else {
            componentEvents[type].AddPair(pair);
        }
    }

    /// <summary>
    /// Subscribe to an event with a callback. This removes all previous subscriptions for this event.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void OverrideSubscription(EventCallback callback, MonoBehaviour component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        ComponentEventsContainer cont = new ComponentEventsContainer();
        cont.AddPair(pair);
        componentEvents.Add(type, cont);
    }
    /// <summary>
    /// Unsubscribes a callback from an event
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="type"></param>
    public static void UnsubscribeFromEvent(EventCallback callback, MonoBehaviour component, EventType type) {

        CallbackComponentPair pair = new CallbackComponentPair(callback, component);

        if (componentEvents.ContainsKey(type)) {
            componentEvents[type].Unsubscribe(pair);
        }
    }

    private static void CallComponentCallbacks(EventType type, string data) {

        ComponentEventsContainer cont;

        componentEvents.TryGetValue(type, out cont);
        cont?.FireIfNotNull(data);
    }
    #endregion


    /// <summary>
    /// Calls all callbacks for event
    /// </summary>
    /// <param name="type"></param>
    public static void FireEvent(EventType type) {
        CallDefaultCallbacks(type, null);
        CallComponentCallbacks(type, null);
    }

    /// <summary>
    /// Calls all callbacks for event with data
    /// </summary>
    /// <param name="type"></param>
    public static void FireEvent(EventType type, string data) {
        CallDefaultCallbacks(type, data);
        CallComponentCallbacks(type, data);
    }
}