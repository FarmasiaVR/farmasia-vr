using UnityEngine;

// Maybe add support for multiple subscriptions????
public class CollisionSubscription : MonoBehaviour {

    #region fields
    private CollisionListener ColListener { get; set; } = new CollisionListener();
    private TriggerListener TrigListener { get; set; } = new TriggerListener();
    #endregion

    #region Triggers
    private void OnTriggerEnter(Collider collider) {
        TrigListener.InvokeOnEnter(collider);
    }
    private void OnTriggerStay(Collider collider) {
        TrigListener.InvokeOnStay(collider);
    }
    private void OnTriggerExit(Collider collider) {
        TrigListener.InvokeOnExit(collider);
    }
    #endregion

    #region Collisions
    private void OnCollisionEnter(Collision collision) {
        ColListener.InvokeOnEnter(collision);
    }
    private void OnCollisionStay(Collision collision) {
        ColListener.InvokeOnStay(collision);
    }
    private void OnCollisionExit(Collision collision) {
        ColListener.InvokeOnExit(collision);
    }
    #endregion

    #region Subscribing
    public static void SubscribeToTrigger(GameObject g, TriggerListener listener) {
        (g.GetComponent<CollisionSubscription>() ?? g.AddComponent<CollisionSubscription>()).TrigListener = listener;
    }

    public static void SubscribeToCollision(GameObject g, CollisionListener listener) {
        (g.GetComponent<CollisionSubscription>() ?? g.AddComponent<CollisionSubscription>()).ColListener = listener;
    }
    #endregion
}
