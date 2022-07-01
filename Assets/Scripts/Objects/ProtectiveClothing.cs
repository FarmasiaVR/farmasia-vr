public class ProtectiveClothing : GeneralItem {

    public string type;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
    }

    // Change this to OnTriggerEnter with the players collider
    public override void OnGrabStart(Hand hand) {
        base.OnGrab(hand);
        Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(this));
    }
}
