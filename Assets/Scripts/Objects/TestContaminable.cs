using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestContaminable : Interactable {

    private GeneralItem states;

    protected override void Start() {
        base.Start();
        states = gameObject.GetComponent<GeneralItem>();
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
    }

    private void OnCollisionEnter(Collision coll) {
        GameObject collisionObject = coll.gameObject;
        if (collisionObject.tag == "Floor") {
            states.DisableFlags(ItemState.Status.Clean);
        }
        Logger.PrintVariables("Clean", states.GetFlag(ItemState.Status.Clean));
    }
}
