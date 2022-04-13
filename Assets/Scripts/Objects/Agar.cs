using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class Agar : Interactable {

    private bool fingers = false;
    private bool thumb = false; 
    private bool isLeft;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        string handIs = hand.HandType.ToString();
        if(!fingers) {
            base.Interact(hand);
            if (handIs.Equals("LeftHand")) {
                isLeft = true;
            } else {
                isLeft = false;
            }
            fingers = true;
            Logger.Print("fingerprints given");
        } else if (!thumb) {
            base.Interact(hand);
            thumb = true;
            Logger.Print("thumb print given");

            if (handIs.Equals("LeftHand")) {
                Events.FireEvent(EventType.FingerprintsGivenL);
                if (!isLeft){
                    Logger.Print("mokasit");
                } else {
                    Logger.Print("oikein meni");
                }
            } else {
                Events.FireEvent(EventType.FingerprintsGivenR);
                if (isLeft){
                    Logger.Print("mokasit");
                } else {
                    Logger.Print("oikein meni");
                }
            } 
            thumb = true;
        } else {
            Logger.Print("excess touch");
            // Events.FireEvent(EventType.FingerprintsGiven);
        }
    }
}
