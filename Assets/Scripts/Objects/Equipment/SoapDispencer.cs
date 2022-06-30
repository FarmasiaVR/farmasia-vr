using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

public class SoapDispencer : Interactable {

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

    public override void Interact(Hand hand) {
        base.Interact(hand);

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
