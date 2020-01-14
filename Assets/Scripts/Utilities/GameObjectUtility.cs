using System;
using UnityEngine;

public class GameObjectUtility {

    public static T EnsureComponent<T>(GameObject obj) where T : Component {
        if (obj == null) {
            throw new ArgumentNullException("Argument 'obj' must not be null");
        }

        if (obj.GetComponent<T>() != null) {
            return obj.GetComponent<T>();
        }

        return obj?.AddComponent<T>();
    }

    public static T[] EnsureComponents<T>(GameObject obj, int count) where T : Component {
        if (obj == null) {
            throw new ArgumentNullException("Argument 'obj' must not be null");
        }

        if (obj.GetComponent<T>() == null) {
            for (int i = 0; i < count; i++) {
                obj.AddComponent<T>();
            }
        }

        return obj.GetComponents<T>();
    }
}