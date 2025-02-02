﻿using UnityEngine;
using System.Collections;
using System;

public class WritingSubmit : DragAcceptable {

    public Action onSelect;

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Chose: submit");
        onSelect();
    }

    public void Interact() {
        Logger.Print("Chose: submit");
        onSelect();
    }
}