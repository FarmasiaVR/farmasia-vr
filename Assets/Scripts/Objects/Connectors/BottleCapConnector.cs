using UnityEngine;
using System.Collections;

public class BottleCapConnector : AttachmentConnector {
    public override ItemConnection Connection { get; set; }

    public BottleCapConnector(BottleCap cap, GameObject collider) : base(cap.transform) {
        GeneralItem = cap;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    protected override InteractState AttachState => InteractState.CapAttached;

    public override void ConnectItem(Interactable interactable) {
        if (interactable.IsAttached) {
            return;
        }
        bool itemGrabbed = interactable.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(interactable) : null;

        if (itemGrabbed) {
            interactable.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(interactable?.gameObject);

        if (itemGrabbed) {
            itemHand.InteractWith(interactable, false);
        }
    }

    public override void OnReleaseItem() {
        attached.Interactable.Interactors.ResetCap();

        // Attach state might need to change
        attached.Interactable.State.Off(AttachState);
        attached.Interactable.Type.On(InteractableType.Grabbable);
        this.GeneralItem.Type.On(InteractableType.Grabbable);
        ReplaceObject(null);
    }

    protected override void AttachEvents(GameObject intObject) {
        
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetCap(GeneralItem as BottleCap);
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
