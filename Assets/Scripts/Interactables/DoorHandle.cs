public class DoorHandle : Interactable {

    #region fields
    private Hand hand;
    private OpenableDoor door;

    #endregion

    protected override void Start() {
        base.Start();
        door = transform.parent.GetComponent<OpenableDoor>();
        SetFlags(true, InteractableType.Interactable);
    }

    private void Update() {
        if (State == InteractState.Grabbed) {
            door.SetByHandPosition(hand);
        }
    }

    private void UpdatePosition() {
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Door interact");

        door.SetAngleOffset(hand.coll.transform.position);

        this.hand = hand;
        State = InteractState.Grabbed;
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        this.hand = null;
        State = InteractState.None;
        door.ReleaseDoor();
    }
}
