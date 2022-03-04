public class PipeConnectorButton : Interactable {

    protected override void Start() {
        base.Start();

        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        Logger.Print("Attach medicin waste pipe to pump");
        Events.FireEvent(EventType.AttachPipe);
    }
}