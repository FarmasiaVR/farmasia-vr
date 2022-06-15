using UnityEngine.Events;

public class MainMenuButton : Interactable {

    public UnityEvent onActivate;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        onActivate.Invoke();
    }
}
