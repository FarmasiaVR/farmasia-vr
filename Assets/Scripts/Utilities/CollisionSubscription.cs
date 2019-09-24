using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region Triggers
    private void OnTriggerEnter(Collider collider) {
        T_Enter?.Invoke(collider);
    }
    private void OnTriggerExit(Collider collider) {
        T_Exit?.Invoke(collider);
    }
    #endregion

    public static void SubscribeToTrigger(GameObject g, TriggerCallback enter, TriggerCallback stay, TriggerCallback exit) {

        CollisionSubscription coll = g.AddComponent<CollisionSubscription>();

        coll.SetTriggerCallbacks(enter, stay, exit);
    }
}
