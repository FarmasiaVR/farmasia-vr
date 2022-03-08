using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilter : ConnectableItem {
    public override AttachmentConnector Connector { get; set; }

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.PumpFilter;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);

        Connector = new PumpFilterConnector(this, transform.Find("Pump Collider").gameObject);
        Connector.Subscribe();
    }

    public void ReleaseItem() {
        Connector.Connection?.Remove();
    }
}
