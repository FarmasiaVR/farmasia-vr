using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintCloseButton : DragAcceptable {
    protected override void Activate() {

        if (Activated) {
            return;
        }

        Activated = true;

        grabbed = false;

        HintText hint = transform.parent.GetComponent<HintText>();
        transform.parent = null;

        hint.DestroyHint();
        SafeDestroy();
    }
}
