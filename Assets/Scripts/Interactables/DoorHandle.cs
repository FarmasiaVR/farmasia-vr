public class DoorHandle : Interactable {

    #region Fields
    private Hand hand;
    private OpenableDoor door;
    #endregion

    protected override void Start() {
        base.Start();

        door = transform.parent.GetComponent<OpenableDoor>();
        Type.Set(InteractableType.Interactable);
    }

    private void Update() {
        if (State == InteractState.Grabbed) {
            door?.SetByHandPosition(hand);
            Events.FireEvent(EventType.HandleGrabbed, CallbackData.NoData());
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        door.BreakAtLongDistance = !hand.RemoteGrabbing;

        door?.SetAngleOffset(hand.ColliderPosition);

        this.hand = hand;
        State.On(InteractState.Grabbed);
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        this.hand = null;
        State.Off(InteractState.Grabbed);
        door?.ReleaseDoor();
    }
}
