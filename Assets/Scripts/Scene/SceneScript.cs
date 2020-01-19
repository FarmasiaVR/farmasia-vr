using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour {

    protected virtual void Awake() {
        Events.Reset();
    }

    protected virtual void Start() {

        HintBox.Init(true);
    }
}