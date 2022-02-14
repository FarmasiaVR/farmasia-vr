using UnityEngine;
using System.Collections;

public class BottleCap : GeneralItem {

    public BottleCapConnector Connector { get; private set; }

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Needle;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);

        Connector = new BottleCapConnector(this, transform.parent.Find("Bottle Opening").gameObject);
        Connector.Subscribe();
    }

    public void ReleaseItem() {
        Connector.Connection?.Remove();
    }
}
