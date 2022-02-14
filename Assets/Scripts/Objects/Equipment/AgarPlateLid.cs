
public class AgarPlateLid : GeneralItem {

    public AgarPlateLidConnector Connector { get; private set; }

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.AgarPlateLid;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);

        Connector = new AgarPlateLidConnector(this, transform.Find("Bottom Collider").gameObject);
        Connector.Subscribe();
    }
    public void ReleaseItem() {
        Connector.Connection?.Remove();
    }
}
