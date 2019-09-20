public class DoorHandle : Interactable {

    private Hand hand;
    private OpenableDoor door;
    private bool grabbed;

    protected override void Start() {
        base.Start();
        door = transform.parent.GetComponent<OpenableDoor>();
        type = InteractableType.Interactable;
    }

    private void Update() {
        if (grabbed) {
            UpdatePosition();
        }
    }

    private void UpdatePosition() {
        door.SetByHandPosition(hand);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Door interact");

        door.SetAngleOffset(hand.transform.position);

        this.hand = hand;
        grabbed = true;
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        this.hand = null;
        grabbed = false;
        door.ReleaseDoor();
    }
}
