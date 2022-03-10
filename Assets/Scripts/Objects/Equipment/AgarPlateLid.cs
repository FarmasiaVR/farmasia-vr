
using UnityEngine;

public class AgarPlateLid : ConnectableItem {

    public override AttachmentConnector Connector { get; set; }

    [SerializeField]
    private GameObject BottomObject;

    private string variant;

    public string Variant
    {
        get { return variant; }
    }

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);

        ItemDisplay display = gameObject.GetComponent(typeof(ItemDisplay)) as ItemDisplay;
        variant = display.Text;

        Connector = new SimpleAttachmentConnector(this, transform.Find("Bottom Collider").gameObject) {
            CanConnect = (interactable) => {
                return interactable is AgarPlateBottom;
            },
            AfterRelease = (interactable) => {
                Events.FireEvent(EventType.PlateOpened, CallbackData.Object(interactable));
            }
        };
        Connector.Subscribe();

        var Bottom = BottomObject.GetComponent<Interactable>();

        Connector.ConnectItem(Bottom);
    }

    public void ReleaseItem() {
        Connector.Connection?.Remove();
    }

    public void FixedUpdate() {
        if (IsGrabbed && (IsAttached || Connector.HasAttachedObject)) {
            Type.Off(InteractableType.Grabbable);
            DisableHighlighting = true;
        } else {
            Type.On(InteractableType.Grabbable);
            DisableHighlighting = false;
        }
    }
}
