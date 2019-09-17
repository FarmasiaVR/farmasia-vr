using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrabInteractItem : Interactable {

    protected override void Start() {
        base.Start();

        type = InteractableType.GrabbableAndInteractable;
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        print("Interact with object");
    }
    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        print("Uninteract with object");
    }
}
