using UnityEngine;
using System.Collections;

public class BottleCap : ConnectableItem {

    public override AttachmentConnector Connector { get; set; }

    [SerializeField]
    private GameObject BottomObject;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);

        Connector = new BottleCapConnector(this, transform.Find("Bottom Collider").gameObject);
        Connector.Subscribe();

        var Bottom = BottomObject.GetComponent<Interactable>();

        Connector.ConnectItem(Bottom);
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        RemoveCap();
    }

    public void RemoveCap() {
        if (Connector.HasAttachedObject) {
            var bottle = Connector.AttachedInteractable;
            Connector.Connection?.Remove();
            CoroutineUtils.StartThrowingCoroutine(this, DisableAttachingForAWhile(bottle));
        }
    }

    IEnumerator DisableAttachingForAWhile(Interactable bottle) {
        bottle.Type.Off(InteractableType.Attachable);
        yield return new WaitForSeconds(2);
        bottle.Type.On(InteractableType.Attachable);
        yield break;
    }
}
