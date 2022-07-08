using UnityEngine;

public class HandWashingLiquid : Interactable {

    public string type;

    AudioSource handWashingSound;

    private bool running = false;

    // public GameObject Effect;
    new ParticleSystem particleSystem;


    protected override void Start() {
        base.Start();
        // Type.On(InteractableType.Interactable);
        Type.Set(InteractableType.Interactable);

        handWashingSound = gameObject.GetComponent<AudioSource>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    public override void Interact(Hand hand) {

        base.Interact(hand);

        Events.FireEvent(EventType.WashingHands, CallbackData.Object(this));

        if (!running) {
            running = true;
            // Logger.Print("Soap ON!");
            PlayFX();

        }
        running = false;

    }

    /*
     * OnTriggerEnter or on collision?
    private void OnCollisionEnter(Collision collision) {
        Events.FireEvent(EventType.WashingHands, CallbackData.Object(this));

        if (!running) {
            running = true;
            // Logger.Print("Soap ON!");
            PlayFX();

        }
        running = false;

    }
    */

    public void PlayFX() {
        if (particleSystem != null) particleSystem.Play();
        if (handWashingSound != null) handWashingSound.Play();
    }
}
