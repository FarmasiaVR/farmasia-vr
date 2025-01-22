﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalpel : GeneralItem {

    [SerializeField]
    private Cover cover;

    [SerializeField]
    private Collider blade;


    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        objectType = ObjectType.Scalpel;
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();

        cover.OnCoverOpen = (hand) => {
            Events.FireEvent(EventType.ScalpelCoverOpened, CallbackData.Object(this));
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

    public void OpenCoverXr()
    {
        Events.FireEvent(EventType.ScalpelCoverOpened, CallbackData.Object(this));
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        cover.OpenCoverWithHand(hand);
    }
}
