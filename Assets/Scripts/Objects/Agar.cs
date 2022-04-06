using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class Agar : Interactable {

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Events.FireEvent(EventType.FingerprintsGiven);
    }
}
