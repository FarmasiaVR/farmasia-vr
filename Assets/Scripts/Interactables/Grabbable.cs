public class Grabbable : Interactable {

    public override void Interact(Hand hand) {
        throw new System.NotImplementedException();
    }

    protected override void Start() {
        base.Start();
        Types.On(InteractableType.Grabbable);
    }
}
