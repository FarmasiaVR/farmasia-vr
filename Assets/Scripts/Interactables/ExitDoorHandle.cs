public class ExitDoorHandle : Interactable {

    #region fields
    private Hand hand;
    private ExitDoor door;

    #endregion

    protected override void Start() {
        base.Start();
        door = transform.parent.GetComponent<ExitDoor>();
        Type.Set(InteractableType.Interactable);
    }

    private void Update() {
        if (State == InteractState.Grabbed) {
            door.CheckExitPermission();
        }
    }

    private void UpdatePosition() {
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Door interact");
        this.hand = hand;
        State.On(InteractState.Grabbed);
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        this.hand = null;
        State.Off(InteractState.Grabbed);
    }
}