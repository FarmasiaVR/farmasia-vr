using UnityEngine;
using System.Collections;

public class BottleCap : GeneralItem {

    public BottleCapConnector Connector { get; private set; }

    [SerializeField]
    private GameObject BottomObject;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);

        Connector = new BottleCapConnector(this, transform.Find("Bottom Collider").gameObject);
        Connector.Subscribe();

        var Bottom = BottomObject.GetComponent<Interactable>();

        Connector.ConnectItem(Bottom);
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        ReleaseItem();
    }

    public void ReleaseItem() {
        if (IsAttached) {
            Logger.Print("Releasing bottle cap when grabbed");
            Connector.Connection?.Remove();
        }
    }
}
