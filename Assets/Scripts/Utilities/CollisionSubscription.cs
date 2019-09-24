using UnityEngine;

// Maybe add support for multiple subscriptions????
public class CollisionSubscription : MonoBehaviour {

    #region fields
    public delegate void TriggerCallback(Collider collider);
    public delegate void CollisionCallback(Collision collider);

    private TriggerCallback T_Enter, T_Stay, T_Exit;
    private CollisionCallback C_Enter, C_Stay, C_Exit;
    #endregion

    private void SetTriggerCallbacks(TriggerCallback enter, TriggerCallback stay, TriggerCallback exit) {
        T_Enter = enter;
        T_Stay = stay;
        T_Exit = exit;
    }
    private void SetCollisionCallbacks(CollisionCallback enter, CollisionCallback stay, CollisionCallback exit) {
        C_Enter = enter;
        C_Stay = stay;
        C_Exit = exit;
    }

    #region Triggers
    private void OnTriggerEnter(Collider collider) {
        T_Enter?.Invoke(collider);
    }
    private void OnTriggerStay(Collider collider) {
        T_Stay?.Invoke(collider);
    }
    private void OnTriggerExit(Collider collider) {
        T_Exit?.Invoke(collider);
    }
    #endregion

    #region Collisions
    private void OnCollisionEnter(Collision collision) {
        C_Enter?.Invoke(collision);
    }
    private void OnCollisionStay(Collision collision) {
        C_Stay?.Invoke(collision);
    }
    private void OnCollisionExit(Collision collision) {
        C_Exit?.Invoke(collision);
    }
    #endregion

    #region Subscribing
    public static void SubscribeToTrigger(GameObject g, TriggerCallback enter, TriggerCallback stay, TriggerCallback exit) {

        CollisionSubscription coll = g.GetComponent<CollisionSubscription>();
        if (coll == null) {
            coll = g.AddComponent<CollisionSubscription>();
        }

        coll.SetTriggerCallbacks(enter, stay, exit);
    }
    public static void SubscribeToCollision(GameObject g, CollisionCallback enter, CollisionCallback stay, CollisionCallback exit) {

        CollisionSubscription coll = g.GetComponent<CollisionSubscription>();
        if (coll == null) {
            coll = g.AddComponent<CollisionSubscription>();
        }

        coll.SetCollisionCallbacks(enter, stay, exit);
    }
    #endregion
}
