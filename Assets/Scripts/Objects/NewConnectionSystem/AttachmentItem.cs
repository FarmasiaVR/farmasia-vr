using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttachmentItem : GeneralItem
{
    public bool Attached = false;
    public ReceiverItem ParentReceiver = null;
    public float SnapDistance;

    public Func<Interactable, bool> CanConnect = (interactable) => true;
    public Action<Interactable> AfterConnect = (interactable) => { };
    public Action<Interactable> AfterRelease = (interactable) => { };

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
                var receiver = GetParent() as ReceiverItem;
                receiver?.PrepareForDisconnect(hand, this);

                yield return null;
                yield return null;

                hand.InteractWith(this);

                AfterRelease(receiver);

                break;
            }

            yield return null;
        }
    }

    public IEnumerator WaitForHandDisconnect(ReceiverItem receiver) {
        if (IsGrabbed) {
            grabbingHand.Uninteract();
        }

        yield return null;

        RigidbodyContainer.Disable();

        yield return null;

        transform.SetParent(receiver.transform);
        transform.position = receiver.transform.TransformPoint(receiver.GetComponent<SphereCollider>().center);
        transform.rotation = receiver.transform.rotation;

        AfterConnect(receiver);

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
