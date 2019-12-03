using UnityEngine;

public class GObject : MonoBehaviour {

    [SerializeField]
    SceneTypes scene = SceneTypes.MainMenu;

    private void Awake() {
        G.Instance.Progress.SetSceneType(scene);
    }


    private void Update() {
        G.Instance.Update(Time.deltaTime);
    }
}