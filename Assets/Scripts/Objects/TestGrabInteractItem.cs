public class TestGrabInteractItem : Interactable {

    protected override void Start() {
        base.Start();
        SetFlags(true, InteractableType.Grabbable, InteractableType.Interactable);

    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Interact with object");
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        Logger.Print("Uninteract with object");
    }
}
