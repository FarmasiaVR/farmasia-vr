using UnityEngine;

public class Movable : Interactable {

    [SerializeField]
    private bool smooth;
    public bool Smooth { get => smooth; set => smooth = value; }

    protected override void Awake() {
        base.Awake();

        Type.On(InteractableType.Interactable, InteractableType.Grabbable);
    }

    public override void Interacting(Hand hand) {
        base.Interacting(hand);

        if (smooth) {
            transform.position = Vector3.Lerp(transform.position, hand.Offset.position, 0.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, hand.Offset.rotation, 0.5f);
        } else {
            transform.position = hand.Offset.position;
            transform.rotation = hand.Offset.rotation;
        }
    }
}
