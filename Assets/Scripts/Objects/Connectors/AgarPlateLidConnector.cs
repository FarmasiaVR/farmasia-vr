using System.Collections.Generic;
using UnityEngine;

public class AgarPlateLidConnector : AttachmentConnector {

    public override ItemConnection Connection { get; set; }

    public AgarPlateLidConnector(AgarPlateLid lid, GameObject collider) : base(lid.transform) {
        GeneralItem = lid;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    public override void ConnectItem(Interactable interactable) {
        var bottom = interactable as AgarPlateBottom;
        if (bottom == null || bottom.IsAttached) {
            return;
        }

        bool itemGrabbed = bottom.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(bottom) : null;

        if (itemGrabbed) {
            bottom.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(bottom.gameObject);

        if (itemGrabbed) {
            itemHand.InteractWith(bottom, false);
        }
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetConnectableItem(GeneralItem as AgarPlateLid);
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
        AgarPlateBottom bottom = (AgarPlateBottom)attached.Interactable;
        GameObject gameObject = (GameObject)attached.GameObject;

        attached.Interactable.Interactors.ResetConnectableItem();

        // Attach state might need to change
        attached.Interactable.State.Off(AttachState);
        attached.Interactable.Type.On(InteractableType.Grabbable);
        this.GeneralItem.Type.On(InteractableType.Grabbable);
        ReplaceObject(null);
        Events.FireEvent(EventType.PlateOpened, CallbackData.Object(gameObject));
    }
}
