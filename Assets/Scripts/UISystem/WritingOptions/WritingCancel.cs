using UnityEngine;
using System.Collections;
using System;

public class WritingCancel : DragAcceptable {

    public Action onSelect;

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Chose: cancel");
        onSelect();
    }
}