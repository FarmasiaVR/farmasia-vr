using System.Collections.Generic;
using UnityEngine;

public struct CallbackComponentPair {

    #region fields
    public Events.EventEmptyCallback CallbackNoData;
    public Events.EventDataCallback CallbackWithData;
    public Component Component;
    #endregion

    public CallbackComponentPair(Events.EventDataCallback callback, Component component) {
        CallbackNoData = null;
        CallbackWithData = callback;
        Component = component;
    }
    public CallbackComponentPair(Events.EventEmptyCallback callback, Component component) {
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
        hashCode = hashCode * -1521134295 + EqualityComparer<Events.EventDataCallback>.Default.GetHashCode(CallbackWithData);
        hashCode = hashCode * -1521134295 + EqualityComparer<Component>.Default.GetHashCode(Component);
        return hashCode;
    }
}
