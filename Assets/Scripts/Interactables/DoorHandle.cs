public class DoorHandle : Interactable {

    #region fields
    private Hand hand;
    private OpenableDoor door;
    private bool isGrabbed;
    #endregion

    protected override void Start() {
        base.Start();
        door = transform.parent.GetComponent<OpenableDoor>();
        type = InteractableType.Interactable;
    }

    private void Update() {
        if (isGrabbed) {
            UpdatePosition();
        }
    }

    private void UpdatePosition() {
        door.SetByHandPosition(hand);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Door interact");

        door.SetAngleOffset(hand.coll.transform.position);

        this.hand = hand;
        isGrabbed = true;
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        this.hand = null;
        isGrabbed = false;
        door.ReleaseDoor();
    }
}
