using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandle : Interactable {

    private Hand hand;
    private OpenableDoor door;
    private bool grabbed;

    protected override void Start() {
        base.Start();

        door = transform.parent.GetComponent<OpenableDoor>();

        type = GrabType.Interactable;
    }

    private void Update() {
        if (grabbed) {
            UpdatePosition();
        }
    }

    private void UpdatePosition() {
        door.SetByHandPosition(hand.transform.position);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        Logger.Print("Door interact");

        this.hand = hand;

        grabbed = true;
    }
    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        this.hand = null;

        grabbed = false;
        door.ReleaseDoor();
    }


}
