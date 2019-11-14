using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintCloseButton : DragAcceptable {

    public HintText Hint { get; set; }

    protected override void Activate() {

        if (Activated) {
            return;
        }

        Activated = true;

        grabbed = false;

        Hint.DestroyHint();
        SafeDestroy();
    }
}
