using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultiColliderTool {
    
    public static Bounds GetCombinedColliderBounds(GameObject g) {
        Collider c = g.GetComponent<Collider>();
        Bounds combinedBounds = c != null ? c.bounds : new Bounds(Vector3.zero, Vector3.zero);
        Component[] colliders = g.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders) {
            if (combinedBounds.size == Vector3.zero) {
                combinedBounds = collider.bounds;
            } else {
                combinedBounds.Encapsulate(collider.bounds);
            }
        }
        
        return combinedBounds;
    }

    public static bool CheckCollision(GameObject g1, GameObject g2) {
        Bounds b1 = GetCombinedColliderBounds(g1);
        Bounds b2 = GetCombinedColliderBounds(g2);

        return b1.Intersects(b2);
    }

    public static bool CheckCollision(Bounds b1, Bounds b2) {
        return b1.Intersects(b2);
    }
}
