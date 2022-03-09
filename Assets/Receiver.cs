using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

public class Receiver : Attachment
{
    public ObjectType ReceivedObjectType;

    protected LineRenderer LineEffect;
    protected bool SlotOccupied = false;

    protected List<GameObject> PossibleItems;
    protected GameObject NearestItem = null;

    protected override void Awake() {
        base.Awake();

        PossibleItems = new List<GameObject>();
        LineEffect = transform.GetChild(1).GetComponent<LineRenderer>();
    }

    protected GameObject GetNearestItem() {
        if (PossibleItems.Count == 1) return PossibleItems[0];

        GameObject nearestItem = PossibleItems[0];
        float nearestDistance = Vector3.Distance(transform.position, nearestItem.transform.position);

        for (int i = 1; i < PossibleItems.Count; i++) {
            float newDistance = Vector3.Distance(transform.position, PossibleItems[i].transform.position);
            if (newDistance < nearestDistance) {
                nearestDistance = newDistance;
                nearestItem = PossibleItems[i];
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

            Destroy(NearestItem.GetComponent<Rigidbody>());

            NearestItem.transform.SetParent(transform);
            NearestItem.transform.position = transform.position + GetComponent<SphereCollider>().center;
            NearestItem.transform.rotation = transform.rotation;

            Attachment nearestItemAttachment = NearestItem.GetComponent<Attachment>();
            nearestItemAttachment.Attached = true;
            nearestItemAttachment.AttachedInteractable = this;
            
            PossibleItems.Clear();
            NearestItem = null;
        }
    }
}