
public class Needle : ConnectableItem {

    public override AttachmentConnector Connector { get; set; }

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Needle;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);

        Connector = new NeedleConnector(this, transform.Find("Syringe Collider").gameObject);
        Connector.Subscribe();
    }
    public void ReleaseItem() {
        Connector.Connection?.Remove();
    }
}
