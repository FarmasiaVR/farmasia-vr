using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

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
                Events.FireEvent(EventType.BottleClosed, CallbackData.Object(this));
                return interactable is Bottle;
            },
            AfterConnect = (interactable) => {

            },
            AfterRelease = (interactable) => {
                if (tightened)
                    G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep, gameObject);
                tightened = false;
                Events.FireEvent(EventType.BottleOpened, CallbackData.Object(this));
            }
        };

        AttachCap();
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        RemoveCap();
    }

    public void AttachCap() {
        if (!GetComponent<XRGrabInteractable>())
        {
            var Bottom = BottomObject.GetComponent<Interactable>();
            Connector.ConnectItem(Bottom);
        }
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
        yield return new WaitForSeconds(0.5f);
        bottle.Type.On(InteractableType.Attachable);
        yield break;
    }
}
