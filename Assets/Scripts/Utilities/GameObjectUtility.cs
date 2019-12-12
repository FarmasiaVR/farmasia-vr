using UnityEngine;

public class GameObjectUtility {

    public static T EnsureComponent<T>(GameObject obj) where T : Component {
        if (obj?.GetComponent<T>() != null) {
            return obj.GetComponent<T>();
        }

        return obj?.AddComponent<T>();
    }
}
