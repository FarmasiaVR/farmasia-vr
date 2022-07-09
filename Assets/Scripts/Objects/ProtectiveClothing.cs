using UnityEngine;

public class ProtectiveClothing : Grabbable {

    private GameObject[] posters;
    public ClothingType type;
    public GameObject prefab;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
        posters = GameObject.FindGameObjectsWithTag("AsepticClothingPoster");
        foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>();
    }

    // Change this to OnTriggerEnter with the players collider
    // Change this so the object spawns in hand, not the floor and has the necessary attributes like colour
    public override void OnGrabStart(Hand hand) {
        base.OnGrab(hand);
        Destroy(gameObject);
        if (type != ClothingType.LabCoat) Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(this));
        foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>().HighlightText(type);
    }
}
