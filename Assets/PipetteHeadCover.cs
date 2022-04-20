using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipetteHeadCover : GeneralItem {
    private Cover cover;

    private bool coverOn;

    protected void Awake() {
        cover = gameObject.GetComponent<Cover>();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();

        cover.OnCoverOpen = (hand) => {
            Events.FireEvent(EventType.PipetteCoverOpened, CallbackData.Object(this));
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
    }
}