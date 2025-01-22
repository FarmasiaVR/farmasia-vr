using UnityEngine;

public class CollisionListener {

    #region fields
    public delegate void CollisionCallback(Collision collider);
    private CollisionCallback onEnter, onStay, onExit;
    #endregion

    public CollisionListener OnEnter(CollisionCallback onEnter) {
        this.onEnter = onEnter;
        return this;
    }

    public CollisionListener OnStay(CollisionCallback onStay) {
        this.onStay = onStay;
        return this;
    }

    public CollisionListener OnExit(CollisionCallback onExit) {
        this.onExit = onExit;
        return this;
    }

    public void Invoke(CollisionCallback callback, Collision collision) {
        callback?.Invoke(collision);
    } 

    public void InvokeOnEnter(Collision collision) {
        Invoke(onEnter, collision);
    }

    public void InvokeOnStay(Collision collision) {
        Invoke(onStay, collision);
    }

    public void InvokeOnExit(Collision collision) {
        Invoke(onExit, collision);
    }
}