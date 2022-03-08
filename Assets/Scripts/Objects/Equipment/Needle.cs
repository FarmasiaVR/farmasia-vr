
public class Needle : ConnectableItem {

    public override AttachmentConnector Connector { get; set; }

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Needle;
        Type.On(InteractableType.Interactable);

        Connector = new SimpleAttachmentConnector(this, transform.Find("Syringe Collider").gameObject) {
            CanConnect = (interactable) => {
                var syringe = interactable as Syringe;
                return syringe != null && !syringe.HasSyringeCap;
            },
            AfterRelease = (interactable) => {
                var syringe = interactable as Syringe;
                if (syringe.Container.Amount > 0) {
                    Events.FireEvent(EventType.FinishedTakingMedicineToSyringe, CallbackData.Object(syringe));
                }
            }
        };

        Connector.Subscribe();
    }
    public void ReleaseItem() {
        Connector.Connection?.Remove();
    }
}
