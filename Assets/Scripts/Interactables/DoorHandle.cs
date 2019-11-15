using UnityEngine;
using UnityEngine.Assertions;

public class DoorHandle : Interactable {

    #region Fields
    private Hand hand;
    private OpenableDoor door;
    private Animator animator;
    #endregion


    protected override void Start() {
        base.Start();

        door = transform.parent.GetComponent<OpenableDoor>();
        Type.Set(InteractableType.Interactable);
        animator = transform.GetChild(1).GetComponent<Animator>();
        Assert.IsNotNull(animator);
    }

    private void Update() {
        if (State == InteractState.Grabbed) {
            door.SetByHandPosition(hand);
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Door interact");

        door.SetAngleOffset(hand.ColliderPosition);

        this.hand = hand;
        State.On(InteractState.Grabbed);
        animator.SetTrigger("next");
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        this.hand = null;
        State.Off(InteractState.Grabbed);
        door.ReleaseDoor();
        animator.SetTrigger("next");
    }
}
