using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionIgnore {

    public static void IgnoreCollisions(Transform a, Transform b, bool ignore) {

        Collider[] goA_colliders = a.GetComponentsInChildren<Collider>();
        Collider[] goB_colliders = b.GetComponentsInChildren<Collider>();

        if (goA_colliders.Length == 0 || goB_colliders.Length == 0) {
            return;
        }

        foreach (Collider cA in goA_colliders) {
            foreach (Collider cB in goB_colliders) {
                if (cA.enabled && cB.enabled) {
                    Physics.IgnoreCollision(cA, cB, ignore);
                }
            }
        }
    }
}
