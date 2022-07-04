using UnityEngine;

public class GObject : MonoBehaviour {

    [SerializeField]
    SceneTypes scene = SceneTypes.MainMenu;

    private void Start() {
        G.Instance.ResetProgressManager();
        G.Instance.Progress.SetSceneType(scene);
        G.Instance.CurrentSceneType = scene;
    }

    private void Update() {
        G.Instance.Update(Time.deltaTime);
    }
}
