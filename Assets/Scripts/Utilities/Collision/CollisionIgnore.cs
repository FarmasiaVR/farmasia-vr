using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionIgnore {

    public static void IgnoreCollisions(Transform a, Transform b, bool ignore) {
        Collider coll = a.GetComponent<Collider>();

        if (coll != null) {
            IgnoreCollisionsCollider(coll, b, ignore);
        }

        foreach (Transform child in a) {
            IgnoreCollisions(child, b, ignore);
        }
    }

    private static void IgnoreCollisionsCollider(Collider a, Transform b, bool ignore) {
        Collider coll = b.GetComponent<Collider>();

        if (coll != null) {
            Physics.IgnoreCollision(a, coll, ignore);
            foreach (Transform child in b) {
                IgnoreCollisionsCollider(a, child, ignore);
            }
        }
    }
}
