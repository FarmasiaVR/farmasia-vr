using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glasses : Grabbable {

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
    }

    // Temporary
    public override void OnGrabStart(Hand hand) {
        base.OnGrab(hand);
        Events.FireEvent(EventType.CleaningGlasses, CallbackData.Object(this));
    }
}
