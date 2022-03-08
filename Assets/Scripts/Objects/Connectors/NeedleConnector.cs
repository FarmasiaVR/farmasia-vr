using System.Collections.Generic;
using UnityEngine;

public class NeedleConnector : AttachmentConnector {

    public override ItemConnection Connection { get; set; }

    protected override InteractState AttachState => InteractState.ConnectableAttached;


    public NeedleConnector(Needle needle, GameObject collider) : base(needle.transform) {
        GeneralItem = needle;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    public override void ConnectItem(Interactable interactable) {
        var syringe = interactable as Syringe;
        if (syringe == null || syringe.HasSyringeCap) {
            Logger.Warning("Trying to attach needle to syringe with a cap");
            return;
        }

        if (syringe.IsAttached) {
            return;
        }

        bool itemGrabbed = syringe.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(syringe) : null;

        if (itemGrabbed) {
            syringe.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(syringe.gameObject);

        if (itemGrabbed) {
            itemHand.InteractWith(syringe, false);
        }
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetConnectableItem(GeneralItem as Needle);
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

        Syringe syringe = (Syringe)attached.Interactable;

        attached.Interactable.Interactors.ResetConnectableItem();

        // Attach state might need to change
        attached.Interactable.State.Off(AttachState);
        ReplaceObject(null);
        if (syringe.Container.Amount > 0) {
            Events.FireEvent(EventType.FinishedTakingMedicineToSyringe, CallbackData.Object(syringe));
        }
    }
}
