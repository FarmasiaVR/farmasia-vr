using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable {
    public override void Interact() {
        throw new System.NotImplementedException();
    }

    protected override void Start() {
        base.Start();
        type = GrabType.Grabbable;
    }

}
