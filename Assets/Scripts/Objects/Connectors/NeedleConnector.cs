using System.Collections.Generic;
using UnityEngine;

public class NeedleConnector : AttachmentConnector {


    #region Fields
    public override ItemConnection Connection { get; set; }
    #endregion

    public NeedleConnector(Needle needle, GameObject collider) : base(needle.transform) {
        GeneralItem = needle;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable) {
        Logger.Print("Connect item to needle: " + interactable.name);

        bool itemGrabbed = interactable.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(interactable) : null;

        if (itemGrabbed) {
            interactable.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(interactable?.gameObject);

        // RETHINK
        if (itemGrabbed) {
            Vector3 pos = itemHand.Offset.position;
            Quaternion rot = itemHand.Offset.rotation;

            itemHand.InteractWith(interactable);

            itemHand.Offset.position = pos;
            itemHand.Offset.rotation = rot;
        }
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetNeedle(GeneralItem as Needle);
    }

    protected override void AttachEvents(GameObject intObject) {
    }

    protected override void SnapObjectPosition() {
        Transform obj = attached.GameObject.transform;
        Transform coll = Collider.transform;
        Transform luerlockPos = LuerlockAdapter.LuerlockPosition(obj);

        obj.up = coll.up;

        Vector3 offset = coll.position - luerlockPos.position;
        obj.position += offset;
    }
    #endregion

    #region Releasing
    public override void OnReleaseItem() {
        attached.Interactable.Interactors.SetNeedle(null);

        // Attach state might need to change
        attached.Interactable.State.Off(InteractState.LuerlockAttached);
        ReplaceObject(null);
    }
    #endregion
}
