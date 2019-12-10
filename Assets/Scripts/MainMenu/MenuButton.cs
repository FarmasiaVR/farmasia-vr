using UnityEngine;

public class MenuButton : Interactable {
    [SerializeField]
    private MenuInterface menu;

    [SerializeField]
    private SceneTypes scene;

    [SerializeField]
    private bool isCloseButton = false;

    private SceneLoader changer;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
        changer = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
    }
    public override void Interact(Hand hand) {
        base.Interact(hand);
        if (isCloseButton) {
            menu.Close();
        }
        changer.SwapScene(scene);
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
    }
}
