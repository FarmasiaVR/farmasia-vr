using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour {

    private string scene;

    public CameraFadeController fadeController;

    public void SwapScene(SceneTypes type) {
        ChangeScene(GameScenes.GetName(type));
    }

    public void SwapScene(int type) => SwapScene((SceneTypes) type);

    private void ChangeScene(string name) {
        scene = name;
        FadeOutScene();
    }

    public void FadeOutScene() {
        fadeController.onFadeOutComplete.AddListener(OnFadeComplete);
        fadeController.BeginFadeOut();
    }

    private void OnFadeComplete() {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        LoadScene();
    }

    private void LoadScene() {
        Events.Reset();
        if (scene.Equals("Restart")) {
            Logger.PrintVariables("Restarting current scene", scene);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            Logger.PrintVariables("Loading scene", scene);
            SceneManager.LoadScene(scene);
        }
    }
}
