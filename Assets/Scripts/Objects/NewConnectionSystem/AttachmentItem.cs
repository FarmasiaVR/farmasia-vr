using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentItem : GeneralItem
{
    public bool Attached = false;
    public ReceiverItem ParentReceiver = null;
    public float SnapDistance;

    public AttachmentItem GetParent() {
        if (!Attached) return this;
        return ParentReceiver.GetParent();
    }

    public IEnumerator WaitForDistance(Hand hand) {
        Vector3 startPositionDelta = hand.transform.position - hand.Other.transform.position;
        
        while (true) {
            if (VRInput.GetControlUp(hand.HandType, Controls.Grab)) break;

            Vector3 currentPositionDelta = hand.transform.position - hand.Other.transform.position;
            float currentDistance = Vector3.Distance(startPositionDelta, currentPositionDelta);
            
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

    public IEnumerator WaitForHandDisconnect(ReceiverItem receiver) {
        if (IsGrabbed) grabbingHand.Uninteract();

        yield return null;

        RigidbodyContainer.Disable();

        yield return null;

        transform.SetParent(receiver.transform);
        transform.position = receiver.transform.position + receiver.GetComponent<SphereCollider>().center;
        transform.rotation = receiver.transform.rotation;

        Attached = true;
        ParentReceiver = receiver;

        GetComponent<ObjectHighlight>().Unhighlight();
    }

    public virtual void ResetItem() {
        ParentReceiver.SlotOccupied = false;
        Attached = false;
        ParentReceiver = null;
    }
}
