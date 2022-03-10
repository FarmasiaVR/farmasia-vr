using UnityEngine;
using System;

class SimpleAttachmentConnector : AttachmentConnector {
    public override ItemConnection Connection { get; set; }

    protected override InteractState AttachState => InteractState.ConnectableAttached;

    public Func<Interactable, bool> CanConnect = (interactable) => true;
    public Action<Interactable> AfterConnect = (interactable) => { };
    public Action<Interactable> AfterRelease = (interactable) => { };

    public SimpleAttachmentConnector(ConnectableItem item, GameObject collider) : base(item.transform) {
        GeneralItem = item;
        attached = new AttachedObject();
        this.Collider = collider;
        Subscribe();
    }

    public override void ConnectItem(Interactable interactable) {
        if (!CanConnect(interactable)) {
            return;
        }

        if (interactable.IsAttached) {
            return;
        }

        bool itemGrabbed = interactable.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(interactable) : null;

        if (itemGrabbed) {
            interactable.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(interactable.gameObject);

        if (itemGrabbed) {
            itemHand.InteractWith(interactable, false);
        }
        AfterConnect(interactable);
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetConnectableItem(GeneralItem as ConnectableItem);
    }

    protected override void AttachEvents(GameObject intObject) {
        G.Instance.Audio.Play(AudioClipType.LockedItem);
    }

    protected override void SnapObjectPosition() {
        Transform obj = attached.GameObject.transform;
        Transform coll = Collider.transform;
        Transform luerlockPos = LuerlockAdapter.LuerlockPosition(obj);

        Vector3 pivot = Vector3.Cross(coll.up, obj.up);
        obj.Rotate(pivot, -Vector3.SignedAngle(coll.up, obj.up, pivot), Space.World);

        Vector3 offset = coll.position - luerlockPos.position;
        GeneralItem.transform.position -= offset;
    }

    public override void OnReleaseItem() {
        attached.Interactable.Interactors.ResetConnectableItem();

        attached.Interactable.State.Off(AttachState);
        AfterRelease(attached.Interactable);
        ReplaceObject(null);
    }
}

