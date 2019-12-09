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
    }

    #region Attaching
    public override void ConnectItem(Interactable interactable) {
        Logger.Print("Connect item: " + interactable.name);

        bool luerlockGrabbed = GeneralItem.State == InteractState.Grabbed;
        Hand luerlockHand = luerlockGrabbed ? Hand.GrabbingHand(GeneralItem) : null;

        bool itemGrabbed = interactable.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(interactable) : null;

        if (interactable.State == InteractState.NeedleAttached) {
            Logger.Warning("Cannot connect syringe attached to a needle to luerlock");
            return;
        }

        // Move to ConnectionHandler?
        // Remove current connections
        if (luerlockGrabbed) {
            // Not necessary but more 'clear' for debugging purposes
            Logger.Print("Luerlock is grabbed, removing grab from luerlock");
            Hand.GrabbingHand(GeneralItem).Connector.Connection.Remove();
        }
        if (itemGrabbed) {
            itemHand.Connector.Connection.Remove();
            // interactable.GetComponent<ItemConnection>().Remove();
            Logger.Print("Removing connection from " + interactable.name);
        }


        ReplaceObject(interactable?.gameObject);

        // Move to ConnectionHandler?
        // Add new connections
        if (luerlockGrabbed) {
            luerlockHand.InteractWith(GeneralItem);
        }
        if (itemGrabbed) {
            Vector3 pos = itemHand.Offset.position;
            Quaternion rot = itemHand.Offset.rotation;

            itemHand.InteractWith(interactable);

            itemHand.Offset.position = pos;
            itemHand.Offset.rotation = rot;
        }
    }

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetLuerlockPair(side, GeneralItem as LuerlockAdapter);
    }

    protected override void AttachEvents(GameObject intObject) {
        AudioManager.Play(AudioClipType.LockedItem);
        Events.FireEvent(EventType.AttachLuerlock, CallbackData.Object(intObject));
        Events.FireEvent(EventType.SyringeToLuerlock, CallbackData.Object(intObject));
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
        Events.FireEvent(EventType.SyringeFromLuerlock, CallbackData.Object(attached.GameObject));
        // MonoBehaviour.Destroy(Joint);
        // MonoBehaviour.Destroy(connection);
        attached.Interactable.Interactors.ResetLuerlockPair();
        attached.Interactable.State.Off(AttachState);
        ReplaceObject(null);
    }
    #endregion
}
