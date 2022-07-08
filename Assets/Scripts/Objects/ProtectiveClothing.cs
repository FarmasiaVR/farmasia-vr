using UnityEngine;

public class ProtectiveClothing : Grabbable {

    private GameObject[] posters;
    public string type;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
        posters = GameObject.FindGameObjectsWithTag("AsepticClothingPoster");
        foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>();
    }

    // Change this to OnTriggerEnter with the players collider
    public override void OnGrabStart(Hand hand) {
        base.OnGrab(hand);
        Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(this));
        foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>().HighlightText(type);
        Destroy(gameObject);
    }
}
