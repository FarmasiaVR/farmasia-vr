using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningBottlePCM : GeneralItem
{
    private CleaningBottleColliderPCM cleaningCollider;

    protected override void Awake() {
        base.Awake();

        cleaningCollider = transform.GetChild(1).gameObject.GetComponent<CleaningBottleColliderPCM>();
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);

        bool dPadWestDown = VRInput.GetControlDown(hand.HandType, Controls.GrabInteract);

        if (dPadWestDown) {
            Clean();
        }
    }

    public void Clean() {
        Debug.Log("trying to clean!");
        cleaningCollider.Clean();
    }
}
