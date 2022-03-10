using System.Collections.Generic;
using UnityEngine;

public class ReceiverItem : AttachmentItem
{
    public ObjectType ReceivedObjectType;

    protected LineRenderer LineEffect;
    protected bool SlotOccupied = false;

    public HashSet<GameObject> PossibleItems;
    protected GameObject NearestItem = null;

    protected override void Awake() {
        base.Awake();

        PossibleItems = new HashSet<GameObject>();
        LineEffect = transform.GetChild(1).GetComponent<LineRenderer>();
    }

    protected GameObject GetNearestItem() {
        float nearestDistance = float.MaxValue;
        GameObject nearestItem = null;
        foreach (GameObject item in PossibleItems) {
            float newDistance = Vector3.Distance(transform.position, item.transform.position);
            if (newDistance < nearestDistance) {
                nearestDistance = newDistance;
                nearestItem = item;
            }
        }
        return nearestItem;
    }

    protected void LateUpdate() {
        if (PossibleItems.Count > 0) {
            NearestItem = GetNearestItem();
        }

        UpdateLineEffect(PossibleItems.Count > 0);
    }

    protected void UpdateLineEffect(bool possibleConnectionExists) {
        if (possibleConnectionExists) {
            LineEffect.positionCount = 2;
            LineEffect.SetPosition(0, transform.position);
            LineEffect.SetPosition(1, NearestItem.transform.position);
        } else {
            LineEffect.positionCount = 1;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform.IsChildOf(transform)) {
            return;
        }

        GeneralItem colliderItem = other.gameObject.GetComponentInParent<GeneralItem>();
        if (!SlotOccupied && !(colliderItem == null) && colliderItem.ObjectType == ReceivedObjectType) {
            PossibleItems.Add(colliderItem.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        PossibleItems.Remove(other.gameObject);
        if (PossibleItems.Count == 0) {
            NearestItem = null;
        }
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);

        if (SlotOccupied) return;

        if (NearestItem != null) {
            SlotOccupied = true;

            NearestItem.GetComponent<AttachmentItem>().RigidbodyContainer.Disable();

            NearestItem.transform.SetParent(transform);
            NearestItem.transform.position = transform.position + GetComponent<SphereCollider>().center;
            NearestItem.transform.rotation = transform.rotation;

            AttachmentItem nearestAttachmentItem = NearestItem.GetComponent<AttachmentItem>();
            nearestAttachmentItem.Attached = true;
            nearestAttachmentItem.AttachedInteractable = this;
            
            PossibleItems.Clear();
            NearestItem = null;
        }
    }

    public override void ResetItem() {
        base.ResetItem();
        PossibleItems.Clear();
        NearestItem = null;
    }

    public void PrepareForDisconnect(Hand hand, AttachmentItem itemToDisconnect) {
        itemToDisconnect.transform.SetParent(null);
        itemToDisconnect.transform.position = hand.transform.position;

        itemToDisconnect.RigidbodyContainer.Enable();
        itemToDisconnect.ResetItem();
        ResetItem();
        
        hand.GrabUninteract();
        SlotOccupied = false;
    }
}