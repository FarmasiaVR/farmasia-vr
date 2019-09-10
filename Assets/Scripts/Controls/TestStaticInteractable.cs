using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStaticInteractable : Interactable {

    protected override void Start() {
        base.Start();
        type = GrabType.Interactable;
        gameObject.GetComponent<GeneralItem>().EnableFlags(ItemState.Status.Clean);
    }

    public override void Interact() {
        base.Interact();
        gameObject.GetComponent<GeneralItem>().DisableFlags(ItemState.Status.Clean);
        Logger.Print("Clean", gameObject.GetComponent<GeneralItem>().GetFlag(ItemState.Status.Clean));
    }
}
