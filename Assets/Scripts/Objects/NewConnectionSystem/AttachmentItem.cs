using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentItem : GeneralItem
{
    public bool Attached = false;
    public AttachmentItem AttachedInteractable = null;
    public float SnapDistance;

    public AttachmentItem GetParent() {
        if (!Attached) return this;
        return AttachedInteractable.GetParent();
    }

    public IEnumerator WaitForDistance(Hand hand) {
        while (true) {
            if (VRInput.GetControlUp(hand.HandType, Controls.Grab)) break;
            float currentDistance = Vector3.Distance(hand.transform.position, hand.Other.transform.position);
            if (currentDistance > SnapDistance) {
                (GetParent() as ReceiverItem)?.PrepareForDisconnect(hand, this);

                yield return null;
                yield return null;

                hand.InteractWith(this);
                break;
            }

            yield return null;
        }
    }

    public virtual void ResetItem() {
        Attached = false;
        AttachedInteractable = null;
    }
}
