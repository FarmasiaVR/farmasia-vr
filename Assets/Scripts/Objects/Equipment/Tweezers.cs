using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweezers : ReceiverItem {

    [SerializeField]
    private Cover cover;
    public HandCollider TweezerCollider { get; private set; }

    private bool coverOn;
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

        cover.OnCoverOpen = (hand) => {
            Events.FireEvent(EventType.TweezersCoverOpened, CallbackData.Object(this));
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

    public void openCoverXR()
    {
        Events.FireEvent(EventType.TweezersCoverOpened, CallbackData.Object(this));
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        cover.OpenCoverWithHand(hand);
        coverOn = cover.CoverOn;
        if(VRInput.GetControlUp(base.grabbingHand.HandType, Controls.GrabInteract)) {
            if (!SlotOccupied) return;
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
