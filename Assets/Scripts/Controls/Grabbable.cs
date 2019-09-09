using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable {
    public override void Interact() {
        throw new System.NotImplementedException();
    }

    void Start() {
        type = GrabType.Grabbable;
    }

}
