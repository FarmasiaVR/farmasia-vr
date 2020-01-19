using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public Animator animator;
    private string scene;
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
        if (scene == null) {
            return;
        }
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        LoadScene();
    }

    private void LoadScene() {
        Events.Reset();
        if (scene.Equals("Restart")) {
            Logger.PrintVariables("Restarting current scene", scene);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        Logger.PrintVariables("Loading scene", scene);
        SceneManager.LoadScene(scene);
    }


}
