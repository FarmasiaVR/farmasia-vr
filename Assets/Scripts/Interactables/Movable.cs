using UnityEngine;

public class Movable : Interactable {

    protected override void Awake() {
        base.Awake();

        Type.On(InteractableType.Interactable, InteractableType.Grabbable);
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        transform.position = hand.Smooth.transform.position;
        transform.rotation = hand.Smooth.transform.rotation;
    }
}
