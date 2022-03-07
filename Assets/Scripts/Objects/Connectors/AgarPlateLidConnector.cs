using System.Collections.Generic;
using UnityEngine;

public class AgarPlateLidConnector : AttachmentConnector {

    public override ItemConnection Connection { get; set; }

    protected override InteractState AttachState => InteractState.LidAttached;


    public AgarPlateLidConnector(AgarPlateLid lid, GameObject collider) : base(lid.transform) {
        GeneralItem = lid;
        attached = new AttachedObject();
        this.Collider = collider;
    }

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

    protected override void SetInteractors() {
        attached.Interactable.Interactors.SetAgarPlateLid(GeneralItem as AgarPlateLid);
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
        Logger.Print("Releasing agarplates");

        AgarPlateBottom bottom = (AgarPlateBottom)attached.Interactable;
        GameObject gameObject = (GameObject)attached.GameObject;
        Logger.Print("game object: " + gameObject);

        attached.Interactable.Interactors.ResetAgarPlateLid();

        // Attach state might need to change
        attached.Interactable.State.Off(AttachState);
        attached.Interactable.Type.On(InteractableType.Grabbable);
        this.GeneralItem.Type.On(InteractableType.Grabbable);
        ReplaceObject(null);
        Events.FireEvent(EventType.PlateOpened, CallbackData.Object(gameObject));
    }
}
