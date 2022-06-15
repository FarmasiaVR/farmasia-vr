using UnityEngine;

public class MainMenuFunctions : MonoBehaviour {

    private SceneLoader levelChanger;

    private void Start() {
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
    }

    public void LoadScene(int scene) {
        levelChanger.SwapScene((SceneTypes)scene);
    }

    public void ExitGame() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
