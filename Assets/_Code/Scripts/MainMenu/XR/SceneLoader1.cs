﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader_XR : MonoBehaviour {

    private string scene;

    public Animator animator;

    public void SwapScene(SceneTypes type) {
        switch (type) {
            case SceneTypes.Restart:
                ChangeScene("Restart");
                return;
            case SceneTypes.MainMenu:
                ChangeScene("MainMenu");
                return;
            case SceneTypes.MedicinePreparation:
                ChangeScene("MedicinePreparation");
                return;
            case SceneTypes.Tutorial:
                ChangeScene("Tutorial");
                return;
            case SceneTypes.MembraneFilteration:
                ChangeScene("MembraneFilteration");
                return;
            case SceneTypes.ChangingRoom:
                ChangeScene("ChangingRoom");
                return;
        }
    }

    private void ChangeScene(string name) {
        scene = name;
        FadeOutScene();
    }

    public void FadeOutScene() {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        LoadScene();
    }

    private void LoadScene() {
        Events.Reset();
        if (scene.Equals("Restart")) {
            Logger.PrintVariables("Restarting current scene", scene);
            G.Instance.Scene.Restart();
        } else {
            Logger.PrintVariables("Loading scene", scene);
            SceneManager.LoadScene(scene);
        }
    }
}
