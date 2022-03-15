using UnityEngine;
using System.Collections;
using System;

public class FilteringButton : Interactable {

    private bool running = false;
    protected override void Start() {
        base.Start();

        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        if(!running){
            running = true;
            Logger.Print("Pump ON");
        // Events.FireEvent(EventType.StartFilter);
        } else {
            running = false;
            Logger.Print("Pump OFF");
        }
    }


}