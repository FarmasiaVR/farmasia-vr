using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// An item that can attach to a ReceiverItem.
/// Requires RigidBody at GameObject root and LineRenderer at child-index 1
/// </summary>
public class AttachmentItem : GeneralItem
{
    public bool Attached = false;
    public ReceiverItem ParentReceiver = null;
    public float SnapDistance;

    public Func<Interactable, bool> CanConnect = (interactable) => true;
    public Action<Interactable> AfterConnect = (interactable) => { };
    public Action<Interactable> AfterRelease = (interactable) => { };

    /// <summary>
    /// Returns this item or it's attachment root if it has one
    /// </summary>
    public virtual AttachmentItem GetParent() {
        if (!Attached) return this;
        return ParentReceiver.GetParent();
    }

    /// <summary>
    /// A co-routine that scans if the <c>Hands</c> move far enough from each other
    /// while Grabbing button is held down. If so, disconnects this item from it's hierarchy
    /// and places it into the grabbing <c>Hand</c> given as parameter.
    /// </summary>
    /// <param name="hand">The grabbing hand the item will be placed to</param>
    public IEnumerator WaitForDistance(Hand hand) {
        Vector3 startPositionDelta = hand.transform.position - hand.Other.transform.position;
        
        while (true) {
            if (VRInput.GetControlUp(hand.HandType, Controls.Grab)) break;

            Vector3 currentPositionDelta = hand.transform.position - hand.Other.transform.position;
            float currentDistance = Vector3.Distance(startPositionDelta, currentPositionDelta);
            
            if (currentDistance > SnapDistance) {
                var receiver = GetParent() as ReceiverItem;
                receiver?.Disconnect(hand, this);

                yield return null;
                yield return null;

                ResetItem();
                hand.InteractWith(this);

                AfterRelease(receiver);

                break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// A co-routine that removes this item's <c>Hand</c> - item connection and places the item onto a
    /// ReceiverItem's hierarchy as a child. The placement of the item will be
    /// to the center of <c>ReceiverItem's</c> <c>SphereCollider</c>
    /// </summary>
    /// <param name="receiver"></param>
    /// <returns></returns>
    public IEnumerator WaitForHandDisconnectAndConnectItems(ReceiverItem receiver) {
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

    /// <summary>
    /// Make's this item's and it's parent <c>ReceiverItem's</c> field reflect a non-attached state
    /// </summary>
    public virtual void ResetItem() {
        ParentReceiver.SlotOccupied = false;
        Attached = false;
        ParentReceiver = null;
    }

    public void MakeGrabbable(Vector3 itemPosition) {
        transform.SetParent(null);
        transform.position = itemPosition;

        RigidbodyContainer.Enable();
    }
}
