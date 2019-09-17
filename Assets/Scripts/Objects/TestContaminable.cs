using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestContaminable : Interactable {

    private GeneralItem states;

    protected override void Start() {
        base.Start();
        type = InteractableType.GrabbableAndInteractable;
        states = gameObject.GetComponent<GeneralItem>();
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.PrintVariables("Clean", states.GetFlag(ItemState.Status.Clean));
    }

    private void OnCollisionEnter(Collision coll) {
        GameObject collisionObject = coll.gameObject;
        if (collisionObject.tag == "Floor") {
            states.SetFlags(false, ItemState.Status.Clean);
        }
        Logger.Print("Contaminable hit something");
    }
}
