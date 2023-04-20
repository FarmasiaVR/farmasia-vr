using System;
using System.Diagnostics;
using UnityEngine.XR.Interaction.Toolkit;
using Valve.VR.InteractionSystem;
using UnityEngine;
public class Needle : ConnectableItem
{

    public override AttachmentConnector Connector { get; set; }

    protected override void Start()
    {
        base.Start();
        ObjectType = ObjectType.Needle;
        Type.On(InteractableType.Interactable);

        Connector = new SimpleAttachmentConnector(this, transform.Find("Syringe Collider").gameObject)
        {
            CanConnect = (interactable) =>
            {
                var syringe = interactable as Syringe;
                return syringe != null && !syringe.HasSyringeCap;
            },
            AfterRelease = (interactable) =>
            {
                var syringe = interactable as Syringe;
                if (syringe.Container.Amount > 0)
                {
                    Events.FireEvent(EventType.DetachedNeedleFromSyringe, CallbackData.Object(syringe));
                }
            }
        };
    }

    public void ReleaseItem()
    {
        Connector.Connection?.Remove();
    }


    public void setAttachedItemRef(SelectEnterEventArgs args)
    {
        // UnityEngine.Debug.Log("checking needle attach args");
        DisableAttachedObjectCollider socketManager = args.interactorObject.transform.GetComponent<DisableAttachedObjectCollider>();
        if (socketManager)
        {
            // UnityEngine.Debug.Log("found socket manager");
            Syringe syr = socketManager.socketsOwnerInteractable.GetComponent<Syringe>();
            if (syr)
            {
                Connector.attached.Interactable = syr;
                // UnityEngine.Debug.Log("attached needle to syringe!");
            }
        }
    }

    public void setAttachedItemRefNull(SelectExitEventArgs args)
    {
        Connector.attached.Interactable = null;
    }

    public void needleDetachedEvent(Syringe syringe)
    {
        Events.FireEvent(EventType.DetachedNeedleFromSyringe, CallbackData.Object(syringe));
    }
}
