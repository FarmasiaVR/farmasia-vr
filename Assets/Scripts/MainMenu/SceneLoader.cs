using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour {

    private string scene;

    public CameraFadeController fadeController;

    Dictionary<SceneTypes, string> sceneTypeToSceneName = new Dictionary<SceneTypes, string> {
        { SceneTypes.Restart, "Restart" },
        { SceneTypes.MainMenu, "MainMenu" },
        { SceneTypes.MedicinePreparation, "MedicinePreparation 2.0 XR" },
        { SceneTypes.Tutorial, "ControlsTutorial" },
        { SceneTypes.MembraneFilteration, "XR MembraneFilteration 2.0" },
        { SceneTypes.ChangingRoom, "ChangingRoom" },
        { SceneTypes.FireHazard, "Laboratory" },
        { SceneTypes.FireExtinguisherTutorial, "FireExtinguisherTutorial" },
        { SceneTypes.FireBlanketTutorial, "FireBlanketTutorial" },
        { SceneTypes.EyeShowerTutorial, "EyeShowerTutorial" },
        { SceneTypes.EmergencyShowerTutorial, "EmergencyShowerTutorial" },
        { SceneTypes.EmergencyExitTutorial, "EmergencyExitTutorial" },
        { SceneTypes.EmergencyExit, "EmergencyExit" },
        { SceneTypes.EmergencyExit1, "EmergencyExit 1" }
    };

    public void SwapScene(SceneTypes type) {
        ChangeScene(sceneTypeToSceneName[type]);
    }

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
