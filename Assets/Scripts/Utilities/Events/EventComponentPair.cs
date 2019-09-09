using System.Collections.Generic;
using UnityEngine;

public struct CallbackComponentPair {

    public Events.EventCallback CallbackNoData;
    public Events.EventCallbackWithData CallbackWithData;
    public Component Component;

    public CallbackComponentPair(Events.EventCallbackWithData callback, Component component) {
        CallbackNoData = null;
        CallbackWithData = callback;
        Component = component;
    }
    public CallbackComponentPair(Events.EventCallback callback, Component component) {
        CallbackNoData = callback;
        CallbackWithData = null;
        Component = component;
    }

    public override bool Equals(object obj) {

        if (GetType() != obj.GetType()) {
            return false;
        }

        CallbackComponentPair other = (CallbackComponentPair)obj;

        bool type = CallbackWithData == other.CallbackWithData && CallbackNoData == other.CallbackNoData;
        bool comp = Component.Equals(other.Component);

        return type && comp;
    }

    public override int GetHashCode() {
        var hashCode = 1022924694;
        hashCode = hashCode * -1521134295 + EqualityComparer<Events.EventCallbackWithData>.Default.GetHashCode(CallbackWithData);
        hashCode = hashCode * -1521134295 + EqualityComparer<Component>.Default.GetHashCode(Component);
        return hashCode;
    }
}