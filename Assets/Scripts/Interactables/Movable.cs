public class Movable : Interactable {

    protected override void Awake() {
        base.Awake();

        Type.On(InteractableType.Interactable, InteractableType.Grabbable);
    }

    public override void Interacting(Hand hand) {
        base.Interacting(hand);

        transform.position = hand.Offset.position;
        transform.rotation = hand.Offset.rotation;
    }
}
