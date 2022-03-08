using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilterConnector : AttachmentConnector
{

    public override ItemConnection Connection { get; set; }

    public PumpFilterConnector(PumpFilter filter, GameObject collider) : base(filter.transform) {
        GeneralItem = filter;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    public override void ConnectItem(Interactable interactable) {
        var pump = interactable as Pump;
        if (pump == null || pump.IsAttached) {
            return;
        }

        bool itemGrabbed = pump.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(pump) : null;

        if (itemGrabbed) {
            pump.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(pump.gameObject);

        if (itemGrabbed) {
            itemHand.InteractWith(pump, false);
        }

        Events.FireEvent(EventType.AttachFilter, CallbackData.Object(pump));
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetConnectableItem(GeneralItem as PumpFilter);
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
        Pump pump = (Pump)attached.Interactable;

        attached.Interactable.Interactors.ResetConnectableItem();

        // Attach state might need to change
        attached.Interactable.State.Off(AttachState);
        ReplaceObject(null);
    }
}
