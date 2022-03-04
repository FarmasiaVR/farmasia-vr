using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningBottle : GeneralItem
{
    private CleaningBottleCollider cleaningCollider;

    protected override void Awake() {
        base.Awake();

        cleaningCollider = transform.GetChild(1).gameObject.GetComponent<CleaningBottleCollider>();
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);

        bool dPadWestDown = VRInput.GetControlDown(hand.HandType, Controls.GrabInteract);

        if (dPadWestDown) {
            cleaningCollider.Clean();
        }
    }
}
