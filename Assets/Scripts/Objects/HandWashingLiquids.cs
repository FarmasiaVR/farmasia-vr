using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

public class HandWashingLiquids : GeneralItem {

    public string type;

    private bool running = false;

    [SerializeField]
    private GameObject soapDispencer;

    public GameObject Effect;
    private new ParticleSystem particleSystem;


    protected override void Start() {
        base.Start();
        particleSystem = Effect.GetComponent<ParticleSystem>();
        Type.Set(InteractableType.Interactable);
    }

    // Change this to OnTriggerEnter with the players collider
    public override void OnGrabStart(Hand hand) {
        base.OnGrab(hand);
        Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(this));

        if (!running) {
            running = true;
            Logger.Print("Soap ON!");
            ApplySoap();

        }
        running = false;
    }

    public void ApplySoap() {
        particleSystem.Play();
    }
}
