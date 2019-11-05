public class LaminarCabinetButton : Interactable {

    protected override void Start_Interactable() {
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        Logger.Print("Button interact");
        Events.FireEvent(EventType.CorrectItemsInLaminarCabinet);
    }
}