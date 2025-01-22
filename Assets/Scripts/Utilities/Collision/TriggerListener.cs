using UnityEngine;

public class TriggerListener {

    #region fields
    public delegate void TriggerCallback(Collider col);
    private TriggerCallback onEnter, onStay, onExit;
    #endregion

    public TriggerListener OnEnter(TriggerCallback onEnter) {
        this.onEnter = onEnter;
        return this;
    }

    public TriggerListener OnStay(TriggerCallback onStay) {
        this.onStay = onStay;
        return this;
    }

    public TriggerListener OnExit(TriggerCallback onExit) {
        this.onExit = onExit;
        return this;
    }

    public void Invoke(TriggerCallback callback, Collider collider) {
        callback?.Invoke(collider);
    } 

    public void InvokeOnEnter(Collider collider) {
        Invoke(onEnter, collider);
    }

    public void InvokeOnStay(Collider collider) {
        Invoke(onStay, collider);
    }

    public void InvokeOnExit(Collider collider) {
        Invoke(onExit, collider);
    }
}