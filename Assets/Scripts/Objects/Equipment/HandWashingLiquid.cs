using UnityEngine;

public class HandWashingLiquid : Interactable {

    public string type;

    private bool running = false;

    public GameObject Effect;
    private new ParticleSystem particleSystem;


    protected override void Start() {
        base.Start();
        // Type.On(InteractableType.Interactable);
        Type.Set(InteractableType.Interactable);

        particleSystem = Effect.GetComponent<ParticleSystem>();
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Events.FireEvent(EventType.WashingHands, CallbackData.Object(this));

        if (!running) {
            running = true;
            // Logger.Print("Soap ON!");
            ApplySoap();

        }
        running = false;
    }

    public void ApplySoap() {
        particleSystem.Play();
    }
}
