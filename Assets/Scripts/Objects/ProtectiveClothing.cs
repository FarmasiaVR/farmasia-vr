using UnityEngine;

public class ProtectiveClothing : Grabbable {

    private GameObject[] posters;
    private bool insideCollider;

    public ClothingType type;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
        posters = GameObject.FindGameObjectsWithTag("AsepticClothingPoster");
        foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>();
    }

    // OnGrab is called every frame
    public override void OnGrab(Hand hand) {
        // This is to make sure we FireEvent only when clothing is grabbed and dragged close to the players body
        if (!insideCollider) return;

        // Sending information to the Events system
        base.OnGrab(hand);
        Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(this));

        // Highlights the equipped item in every aseptic clothing poster found throughout the scene
        foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>().HighlightText(type);

        // Play a sound to indicate a piece of clothing was succesfully equipped
        G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);

        // Setting insideCollider to false to make sure we are not trying to access destroyed object
        insideCollider = false;
        Destroy(gameObject);
    }

    // OnTriggerEnter is called when two GameObjects collide
    private void OnTriggerEnter(Collider other) {
        // Checking if we are colliding with PlayerCollider and updating insideCollider to be true
        // PlayerCollider can be found attached to the VRPlayers camera object
        if (other.CompareTag("PlayerCollider")) insideCollider = true;
    }

    // OnTriggerExit is called when two GameObjects stop colliding
    private void OnTriggerExit(Collider other) {
        // Checking if we stopped colliding with PlayerCollider and updating insideCollider to be false
        if (other.CompareTag("PlayerCollider")) insideCollider = false;
    }
}
