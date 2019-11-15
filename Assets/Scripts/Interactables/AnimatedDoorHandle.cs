using UnityEngine;
using UnityEngine.Assertions;

public class AnimatedDoorHandle : Interactable {

    #region Fields
    private Animator animator;
    #endregion

    protected override void Start() {
        base.Start();
        animator = transform.GetChild(1).GetComponent<Animator>();
        Assert.IsNotNull(animator);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        animator.SetTrigger("next");
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        animator.SetTrigger("next");
    }
}
