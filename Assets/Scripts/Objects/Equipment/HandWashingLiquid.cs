using UnityEngine;

public class HandWashingLiquid : Interactable {

    public string type;

    public float runningTime = 5f;
    private float currentRunningTime;

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
        currentRunningTime = runningTime;
    }

    public override void Interact(Hand hand) {

        base.Interact(hand);

        Events.FireEvent(EventType.WashingHands, CallbackData.Object(this));

        if (!running) {
            running = true;
            // Logger.Print("Soap ON!");
            PlayFX();

        }
        // running = false;

    }


    public void PlayFX() {
        if (particleSystem != null) particleSystem.Play();
        if (handWashingSound != null) handWashingSound.Play();
    }

    void TimerCountdown() {
        currentRunningTime = currentRunningTime - Time.deltaTime;
        if (currentRunningTime <= 0) {
            running = false;
            currentRunningTime = runningTime;
        }
    }

    public bool IsRunning() {
        return running;
    }

    void Update() {
        if (running) {
            TimerCountdown();
        }
    }
}
