using UnityEngine;
using System.Collections;

public class BottleCap : ConnectableItem {

    public override AttachmentConnector Connector { get; set; }

    [SerializeField]
    private GameObject BottomObject;
    
    private bool tightened = true;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);

        Connector = new SimpleAttachmentConnector(this, transform.Find("Bottom Collider").gameObject) {
            CanConnect = (interactable) => {
                return interactable is Bottle;
            },
            AfterRelease = (interactable) => {
                tightened = false;
            }
        };

        var Bottom = BottomObject.GetComponent<Interactable>();

        Connector.ConnectItem(Bottom);
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        RemoveCap();
    }

    public void RemoveCap() {
        if (Connector.HasAttachedObject && !tightened) {
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
