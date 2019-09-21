public class TestGrabInteractItem : Interactable {

    protected override void Start() {
        base.Start();
        type = InteractableType.GrabbableAndInteractable;
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
