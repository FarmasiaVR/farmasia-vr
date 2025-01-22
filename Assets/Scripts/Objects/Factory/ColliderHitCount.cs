using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHitCount : MonoBehaviour {

    private List<CollisionSubscription> subscriptions;

    private int prevCount;
    private int currentCount;
    public bool Inside;

    private Interactable interactable;

    private void Awake() {
        subscriptions = new List<CollisionSubscription>();
    }

    private void Update() {

        Inside = currentCount != prevCount;
        prevCount = currentCount;
    }

    public void SetInteractable(GameObject g) {
        interactable = Interactable.GetInteractable(g.transform);
        currentCount = 0;
        prevCount = -1;
        Inside = true;
    }

    public void SubscribeToCollisions(GameObject g) {

        CollisionSubscription.SubscribeToTrigger(g, new TriggerListener().OnStay((Collider coll) => {
            if (interactable != Interactable.GetInteractable(coll.transform)) {
                return;
            }

            currentCount++;
        }));
    }
}
