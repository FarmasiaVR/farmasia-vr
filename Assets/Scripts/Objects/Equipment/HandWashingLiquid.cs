using UnityEngine;

public class HandWashingLiquid : Interactable {

    public string type;

    public float runningTime = 5f;
    private float currentRunningTime;

    AudioSource handWashingSound;

    private bool running = false;

    new ParticleSystem particleSystem;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);

        handWashingSound = gameObject.GetComponent<AudioSource>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        currentRunningTime = runningTime;
    }

    
    public override void Interact(Hand hand) {
        base.Interact(hand);

        // Should not run if the game is completed.
        TaskType currentTask = G.Instance.Progress.CurrentPackage.CurrentTask.TaskType;
        if (type.Equals("Water") || (currentTask == TaskType.WashHandsInChangingRoom || currentTask == TaskType.WashHandsInPreperationRoom)) {
            if (!running) {
                running = true;
                PlayFX();
            }
        }

        Events.FireEvent(EventType.WashingHands, CallbackData.Object(this));
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
