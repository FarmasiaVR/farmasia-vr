public class LaminarCabinetButton : Interactable {

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        Events.FireEvent(EventType.CorrectItemsInLaminarCabinet);
    }
}