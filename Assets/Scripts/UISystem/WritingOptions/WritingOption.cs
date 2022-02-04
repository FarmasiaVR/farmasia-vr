using UnityEngine;
using System.Collections;

public class WritingOption : DragAcceptable {

    [SerializeField]
    private string optionName = "valinta";

    protected override void Activate() {
        Logger.Print("Activated: " + optionName);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Chose: " + optionName);
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        Logger.Print("Grab started on: " + optionName);
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabStart(hand);
        Logger.Print("Grab ended on: " + optionName);
    }
}
