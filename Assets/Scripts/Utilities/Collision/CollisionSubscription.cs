using System.Collections.Generic;
using UnityEngine;

public class CollisionSubscription : MonoBehaviour {

    #region fields
    private List<CollisionListener> colListeners = new List<CollisionListener>();
    private List<TriggerListener> trigListeners = new List<TriggerListener>();
    #endregion

    #region Triggers
    private void OnTriggerEnter(Collider collider) {
        foreach (TriggerListener listener in trigListeners) {
            listener.InvokeOnEnter(collider);
        }
    }
    private void OnTriggerStay(Collider collider) {
        foreach (TriggerListener listener in trigListeners) {
            listener.InvokeOnStay(collider);
        }
    }
    private void OnTriggerExit(Collider collider) {
        foreach (TriggerListener listener in trigListeners) {
            listener.InvokeOnExit(collider);
        }
    }
    #endregion

    #region Collisions
    private void OnCollisionEnter(Collision collision) {
        foreach (CollisionListener listener in colListeners) {
            listener.InvokeOnEnter(collision);
        }
    }
    private void OnCollisionStay(Collision collision) {
        foreach (CollisionListener listener in colListeners) {
            listener.InvokeOnStay(collision);
        }
    }
    private void OnCollisionExit(Collision collision) {
        foreach (CollisionListener listener in colListeners) {
            listener.InvokeOnExit(collision);
        }
    }
    #endregion

    #region Subscribing
    public static void SubscribeToTrigger(GameObject g, TriggerListener listener) {
        GetOrAddScript(g).trigListeners.Add(listener);
    }

    public static void SubscribeToCollision(GameObject g, CollisionListener listener) {
        GetOrAddScript(g).colListeners.Add(listener);
    }

    private static CollisionSubscription GetOrAddScript(GameObject g) {
        return g.GetComponent<CollisionSubscription>() ?? g.AddComponent<CollisionSubscription>();
    }
    #endregion

    #region Cleanup
    public static void Clear(GameObject g) {
        g.GetComponent<CollisionSubscription>()?.Clear();
    }

    private void Clear() {
        colListeners.Clear();
        trigListeners.Clear();
    }
    #endregion
}
