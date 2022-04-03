using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweezers : GeneralItem {

    [SerializeField]
    private Cover cover;

    private bool coverOn;
    private bool firstCheck;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        objectType = ObjectType.Tweezers;     
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();
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
    }
}
