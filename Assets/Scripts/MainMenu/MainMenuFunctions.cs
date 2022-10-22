using UnityEngine;

public class MainMenuFunctions : MonoBehaviour {

    private SceneLoader levelChanger;
    private int sceneType;

    public GameObject mainMenuButtons;
    public GameObject submenuButtons;
    public static bool startFromBeginning = true;

    // using enum for selecting from multiple game proggrespoints.
    public enum SelectedAutoplay { Beginning, Workspace, CloseSettlePlates, CloseFingertipPlates, FilterHalvesToBottles };
    static public SelectedAutoplay selectedAutoplay = SelectedAutoplay.Beginning;

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

    public void LoadScene(int type) {
        levelChanger.SwapScene((SceneTypes)type);
    }

    public void SetStartingPoint(string startingPoint) {
        // if (startingPoint.Equals("PreperationRoom")) startFromBeginning = true;
        // else if (startingPoint.Equals("WorkspaceRoom")) startFromBeginning = false;
        // else if (startingPoint.Equals("WorkspaceRoom")) startFromBeginning = false;
        if (startingPoint.Equals("PreperationRoom")) {
            startFromBeginning = true;
            selectedAutoplay = SelectedAutoplay.Beginning;
        } else if (startingPoint.Equals("WorkspaceRoom")) {
            startFromBeginning = false;
            selectedAutoplay = SelectedAutoplay.Workspace;
        } else if (startingPoint.Equals("CloseSettlePlates")) {
            startFromBeginning = false;
            selectedAutoplay = SelectedAutoplay.CloseSettlePlates;
        } else if (startingPoint.Equals("CloseFingertipPlates")) {
            startFromBeginning = false;
            selectedAutoplay = SelectedAutoplay.CloseFingertipPlates;
        } else if (startingPoint.Equals("FilterHalvesToBottles")) {
            startFromBeginning = false;
            selectedAutoplay = SelectedAutoplay.FilterHalvesToBottles;
        }

        LoadScene(sceneType);
    }

    public void ExitGame() {
        Application.Quit();
        
        // Disabled for the build
        // UnityEditor.EditorApplication.isPlaying = false;
    }
}
