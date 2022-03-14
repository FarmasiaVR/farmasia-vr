using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilterLid : AttachmentItem
{
    public override AttachmentItem GetParent() {
        return this;
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);

        if (Attached) {
            StartCoroutine(DisconnectCoroutine(hand));
        }
    }

    private IEnumerator DisconnectCoroutine(Hand hand) {
        ParentReceiver.Disconnect(hand, this);

        yield return null;
        yield return null;

        ResetItem();
        hand.InteractWith(this);
    }
}
