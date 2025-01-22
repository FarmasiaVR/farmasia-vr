using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour {

    protected virtual void Awake() {
        Events.Reset();
    }

    protected virtual void Start() {

        HintBox.Init(true);
    }

    public virtual void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}