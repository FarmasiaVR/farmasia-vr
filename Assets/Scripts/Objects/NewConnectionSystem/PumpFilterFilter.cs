using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilterFilter : FilterPart {
    public bool CanBeCut { get => !isCut && Attached && ConnectedItem == null; }
    private bool isCut = false;

    public void Cut() {
        isCut = false;
        Events.FireEvent(EventType.FilterCutted, CallbackData.Object(this));
    }
}
