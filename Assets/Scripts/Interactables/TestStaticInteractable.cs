using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStaticInteractable : Interactable {

    protected override void Start() {
        base.Start();
        type = InteractableType.Interactable;
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        gameObject.GetComponent<GeneralItem>().SetFlags(false, ItemState.Status.Clean);
        Logger.Print("Clean", gameObject.GetComponent<GeneralItem>().GetFlag(ItemState.Status.Clean));
    }
}
