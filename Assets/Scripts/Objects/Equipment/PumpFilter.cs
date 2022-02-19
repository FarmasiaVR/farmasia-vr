using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilter : GeneralItem
{

    public PumpFilterConnector Connector { get; private set; }

    protected override void Start()
    {
        base.Start();
        ObjectType = ObjectType.PumpFilter;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);

        Connector = new PumpFilterConnector(this, transform.Find("Collider").gameObject);
        Connector.Subscribe();
    }
    public void ReleaseItem()
    {
        Connector.Connection?.Remove();
    }
}
