
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarPlateLid : ConnectableItem {

    public override AttachmentConnector Connector { get; set; }

    [SerializeField]
    private GameObject BottomObject;
    private AgarPlateBottom thisPlateBottom;

    [SerializeField]
    private string variant;

    public string Variant
    {
        get { return variant; }
    }

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);

        Connector = new SimpleAttachmentConnector(this, transform.Find("Bottom Collider").gameObject) {
            CanConnect = (interactable) => {
                Events.FireEvent(EventType.AgarPlateClosed, CallbackData.Object(this));
                return interactable is AgarPlateBottom;
            },
            AfterRelease = (interactable) => {
                Events.FireEvent(EventType.PlateOpened, CallbackData.Object(this));
            }
        };

        if (!GetComponent<XRBaseInteractable>())
        {
            var Bottom = BottomObject.GetComponent<Interactable>();
            Connector.ConnectItem(Bottom);
        }
        thisPlateBottom = BottomObject.GetComponent<AgarPlateBottom>();
    }

    public void sendPlateClosedEvent()
    {
        Events.FireEvent(EventType.AgarPlateClosed, CallbackData.Object(this));
        thisPlateBottom.closeAgarPlate();
    }

    public void sendPlateOpenedEvent()
    {
        Events.FireEvent(EventType.PlateOpened, CallbackData.Object(this));
        thisPlateBottom.openAgarPlate();
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
