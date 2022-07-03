using UnityEngine;

public class ProtectiveClothing : GeneralItem {

    private AsepticClothingPoster poster;
    public string type;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
        poster = GameObject.FindGameObjectWithTag("AsepticClothingPoster").GetComponent<AsepticClothingPoster>();
    }

    // Change this to OnTriggerEnter with the players collider
    public override void OnGrabStart(Hand hand) {
        base.OnGrab(hand);
        Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(this));
        poster.HighlightText(type);
    }
}
