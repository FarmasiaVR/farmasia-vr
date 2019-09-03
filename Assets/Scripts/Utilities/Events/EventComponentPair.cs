using UnityEngine;

public struct CallbackComponentPair {

    public Events.EventCallbackWithData Callback;
    public MonoBehaviour Component;

    public CallbackComponentPair(Events.EventCallbackWithData callback, MonoBehaviour component) {
        Callback = callback;
        Component = component;
    }

    public override bool Equals(object obj) {

        if (GetType() != obj.GetType()) {
            return false;
        }

        CallbackComponentPair other = (CallbackComponentPair)obj;

        bool type = Callback == other.Callback;
        bool comp = Component.Equals(other.Component);

        return type && comp;
    }
}