using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Transform Transform { get; private set; }
    public static Camera Camera { get; private set; }

    public void Awake() {
        Transform = transform;
        Camera = Transform.Find("Camera").GetComponent<Camera>();

        if (Transform == null || Camera == null) {
            throw new System.Exception("Player init failed");
        }
    }
}
