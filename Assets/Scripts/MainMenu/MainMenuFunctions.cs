using UnityEngine;

public class MainMenuFunctions : MonoBehaviour {

    private SceneLoader levelChanger;
    private int sceneType;

    public GameObject mainMenuButtons;
    public GameObject submenuButtons;
    public static bool startFromBeginning = true;

    private void Start() {
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
    }

    public void ShowSubmenu(int type) {
        mainMenuButtons.SetActive(false);
        submenuButtons.SetActive(true);
        sceneType = type;
    }

    public void HideSubmenu() {
        mainMenuButtons.SetActive(true);
        submenuButtons.SetActive(false);
    }

    public void LoadScene(SceneTypes scene) {
        levelChanger.SwapScene(scene);
    }

    public void LoadScene(int type) => LoadScene((SceneTypes) type);

    public void SetStartingPoint(string startingPoint) {
        if (startingPoint.Equals("PreperationRoom")) startFromBeginning = true;
        else if (startingPoint.Equals("WorkspaceRoom")) startFromBeginning = false;
        LoadScene(sceneType);
    }

    public void ExitGame() {
        Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false;
    }

    
}
