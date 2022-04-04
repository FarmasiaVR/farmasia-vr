using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweezers : ReceiverItem {

    [SerializeField]
    private Cover cover;
    public HandCollider TweezerCollider { get; private set; }

    private bool coverOn;
    private bool firstCheck;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        objectType = ObjectType.Tweezers;     
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();
        var pos = GetComponent<SphereCollider>().center;
        AfterConnect = (Interactable) => {
            Interactable.transform.Rotate(new Vector3(0, 0, 90));
        };

        AfterRelease = (interactable) => {
            interactable.transform.position = transform.TransformPoint(pos);
        };
    }
    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        cover.EnableOpeningSpots();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);
        cover.DisableOpeningSpots();
    }
    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        cover.OpenCoverWithHand(hand);
        coverOn = cover.CoverOn;
        if (coverOn == false && firstCheck == false) {
            Events.FireEvent(EventType.TweezersCoverOpened, CallbackData.Object(this));
            firstCheck = true;
        }
        if(VRInput.GetControlUp(base.grabbingHand.HandType, Controls.GrabInteract)) {
            Interactable obj = ConnectedItem;
            ConnectedItem.ResetItem();
            Disconnect(hand, ConnectedItem);
            AfterRelease(obj);
        }
    }

    protected override bool WillConnect() {
        if (grabbingHand == null) {
            return false;
        }
        return coverOn == false && VRInput.GetControlDown(base.grabbingHand.HandType, Controls.GrabInteract) && base.WillConnect();
    }
}
