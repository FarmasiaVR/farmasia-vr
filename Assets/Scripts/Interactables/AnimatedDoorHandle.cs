using UnityEngine;
using UnityEngine.Assertions;

public class AnimatedDoorHandle : DoorHandle {

    #region Fields
    private Animator animator;
    public bool IsUp { get => animator.GetBool("isUp"); private set => animator.SetBool("isUp", value); }
    #endregion

    protected override void Start() {
        base.Start();
        animator = transform.GetChild(1).GetComponent<Animator>();
        Assert.IsNotNull(animator);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        IsUp = false;

    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        IsUp = true;
    }
}
