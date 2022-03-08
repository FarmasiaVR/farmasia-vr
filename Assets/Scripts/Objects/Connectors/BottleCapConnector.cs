using UnityEngine;
using System.Collections;

public class BottleCapConnector : AttachmentConnector {
    public override ItemConnection Connection { get; set; }

    public BottleCapConnector(BottleCap cap, GameObject collider) : base(cap.transform) {
        GeneralItem = cap;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    public override void ConnectItem(Interactable interactable) {
        var bottle = interactable as Bottle;
        if (bottle == null || bottle.IsAttached) {
            return;
        }
        bool itemGrabbed = bottle.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(bottle) : null;

        if (itemGrabbed) {
            bottle.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(bottle.gameObject);

        if (itemGrabbed) {
            itemHand.InteractWith(bottle, false);
        }
    }

    public override void OnReleaseItem() {
        attached.Interactable.Interactors.ResetConnectableItem();

        // Attach state might need to change
        attached.Interactable.State.Off(AttachState);
        attached.Interactable.Type.On(InteractableType.Grabbable);
        this.GeneralItem.Type.On(InteractableType.Grabbable);
        ReplaceObject(null);
    }

    protected override void AttachEvents(GameObject intObject) {
        
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetConnectableItem(GeneralItem as BottleCap);
    }

    protected override void SnapObjectPosition() {
        Transform obj = attached.GameObject.transform;
        Transform coll = Collider.transform;
        Transform capPosition = obj.Find("CapPosition");

        Vector3 pivot = Vector3.Cross(coll.up, obj.up);
        obj.Rotate(pivot, -Vector3.SignedAngle(coll.up, obj.up, pivot), Space.World);

        Vector3 offset = coll.position - capPosition.position;
        GeneralItem.transform.position -= offset;
    }
}
