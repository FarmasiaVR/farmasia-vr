using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : GeneralItem
{
    public bool Attached = false;
    public Interactable AttachedInteractable = null;

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        if (Attached) {
            hand.InteractWith(AttachedInteractable, false);
        }
    }
}
