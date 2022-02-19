using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilterConnector : AttachmentConnector
{

    public override ItemConnection Connection { get; set; }

    protected override InteractState AttachState => InteractState.PumpFilterAttached;


    public PumpFilterConnector(PumpFilter filter, GameObject collider) : base(filter.transform)
    {
        GeneralItem = filter;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    public override void ConnectItem(Interactable interactable)
    {

        if (interactable.IsAttached)
        {
            return;
        }

        bool itemGrabbed = interactable.State == InteractState.Grabbed;
        Hand itemHand = itemGrabbed ? Hand.GrabbingHand(interactable) : null;

        if (itemGrabbed)
        {
            interactable.GetComponent<ItemConnection>().Remove();
        }

        ReplaceObject(interactable?.gameObject);

        if (itemGrabbed)
        {
            itemHand.InteractWith(interactable, false);
        }

        Logger.Print("Pump filter connected");
    }

    protected override void SetInteractors()
    {
        attached.Interactable.Interactors.SetPumpFilter(GeneralItem as PumpFilter);
    }

    protected override void AttachEvents(GameObject intObject)
    {
        G.Instance.Audio.Play(AudioClipType.LockedItem);
    }

    protected override void SnapObjectPosition()
    {
        Transform obj = attached.GameObject.transform;
        Transform coll = Collider.transform;
        Transform luerlockPos = LuerlockAdapter.LuerlockPosition(obj);

        Vector3 pivot = Vector3.Cross(coll.up, obj.up);
        obj.Rotate(pivot, -Vector3.SignedAngle(coll.up, obj.up, pivot), Space.World);

        Vector3 offset = coll.position - luerlockPos.position;
        GeneralItem.transform.position -= offset;
    }

    public override void OnReleaseItem()
    {

        Pump pump = (Pump)attached.Interactable;

        attached.Interactable.Interactors.ResetPumpFilter();

        // Attach state might need to change
        attached.Interactable.State.Off(AttachState);
        ReplaceObject(null);
    }
}
