using System.Collections.Generic;
using UnityEngine;

public class LuerlockConnector : AttachmentConnector {

    #region Fields
    public override ItemConnection Connection { get; set; }
    private LuerlockAdapter.Side side;

    protected override InteractState AttachState => InteractState.LuerlockAttached;
    #endregion

    public LuerlockConnector(LuerlockAdapter.Side side, LuerlockAdapter luerlock, GameObject collider) : base(luerlock.transform) {
        GeneralItem = luerlock;
        attached = new AttachedObject();
        this.side = side;
        this.Collider = collider;
        Subscribe();
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable) {
        bool luerlockGrabbed = GeneralItem.State == InteractState.Grabbed;
        Hand luerlockHand = luerlockGrabbed ? Hand.GrabbingHand(GeneralItem) : null;

        bool itemGrabbed = interactable.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(interactable) : null;

        if (interactable.IsAttached) {
            return;
        }

        // Move to ConnectionHandler?
        // Remove current connections
        if (luerlockGrabbed) {
            // Not necessary but more 'clear' for debugging purposes
            Hand.GrabbingHand(GeneralItem).Connector.Connection.Remove();
        }
        if (itemGrabbed) {
            itemHand.Connector.Connection.Remove();
        }


        ReplaceObject(interactable?.gameObject);

        // Move to ConnectionHandler?
        // Add new connections
        if (luerlockGrabbed) {
            luerlockHand.InteractWith(GeneralItem, false);
            luerlockHand.Smooth.DisableInitMode();
        }
        if (itemGrabbed) {
            itemHand.InteractWith(interactable, false);
        }

        ((Syringe)AttachedInteractable).EnableDisplay();
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetLuerlockPair(side, GeneralItem as LuerlockAdapter);
    }

    protected override void AttachEvents(GameObject intObject) {
        G.Instance.Audio.Play(AudioClipType.LockedItem);
        Events.FireEvent(EventType.AttachLuerlock, CallbackData.Object(intObject));
        Events.FireEvent(EventType.SyringeToLuerlock, CallbackData.Object(intObject));
    }

    protected override void SnapObjectPosition() {
        Transform obj = attached.GameObject.transform;
        Transform coll = Collider.transform;
        Transform luerlockPos = LuerlockAdapter.LuerlockPosition(obj);

        Vector3 pivot = Vector3.Cross(coll.up, obj.up);
        obj.Rotate(pivot, -Vector3.SignedAngle(coll.up, obj.up, pivot), Space.World);

        Vector3 offset = coll.position - luerlockPos.position;
        obj.position += offset;
    }
    #endregion

    #region Releasing
    public override void OnReleaseItem() {
        G.Instance.Audio.Play(AudioClipType.LockedItem);
        Events.FireEvent(EventType.SyringeFromLuerlock, CallbackData.Object(attached.GameObject));
        // MonoBehaviour.Destroy(Joint);
        // MonoBehaviour.Destroy(connection);
        attached.Interactable.Interactors.ResetLuerlockPair();
        attached.Interactable.State.Off(AttachState);
        ReplaceObject(null);
    }
    #endregion
}
