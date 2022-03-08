using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilter : ConnectableItem {
    public override AttachmentConnector Connector { get; set; }

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.PumpFilter;
        Type.On(InteractableType.Interactable);

        Connector = new SimpleAttachmentConnector(this, transform.Find("Pump Collider").gameObject) {
            CanConnect = (interactable) => {
                return interactable is Pump;
            },
            AfterConnect = (interactable) => {
                Events.FireEvent(EventType.AttachFilter, CallbackData.Object(interactable));
            }
        };
        Connector.Subscribe();
    }

    public void ReleaseItem() {
        Connector.Connection?.Remove();
    }
}
