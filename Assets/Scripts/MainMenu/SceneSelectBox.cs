using UnityEngine;

public class SceneSelectBox : Interactable {
    [SerializeField]
    GameObject liquid;
    ProgressBar bar;
    [SerializeField]
    private SceneTypes scene;
    private SceneLoader changer;


    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
        bar = liquid.GetComponent<ProgressBar>();
        changer = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
    }

    private void Update() {
        if (bar.Done) {
            changer.SwapScene(scene);
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        bar.grabbing = true;
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        bar.grabbing = false;
    }
}
