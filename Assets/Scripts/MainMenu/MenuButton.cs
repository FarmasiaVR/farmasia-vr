using UnityEngine;

public class MenuButton : Interactable {

    private SceneLoader levelChanger;

    public MenuInterface menu;
    public SceneTypes scene;
    public bool isCloseButton = false;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
    }

    public override void Interact(Hand hand) {
        if (gameObject.activeInHierarchy) {
            base.Interact(hand);
            if (isCloseButton) {
                menu.Close();
            } else {
                levelChanger.SwapScene(scene);
                levelChanger.FadeOutScene();
            }
        }
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
    }
}
