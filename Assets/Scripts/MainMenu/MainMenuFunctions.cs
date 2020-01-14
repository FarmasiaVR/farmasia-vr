using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFunctions : MonoBehaviour {

    private SceneLoader changer;

    private void Start() {
        changer = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
    }

    public void LoadScene(int scene) {
        changer.SwapScene((SceneTypes)scene);
    }

    public void ExitGame() {
        Application.Quit();
    }
}