using System.Collections;
using UnityEngine;

public class FilterHalf : AttachmentItem {
    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        Events.FireEvent(EventType.TouchedFilterWithHand);
    }
}