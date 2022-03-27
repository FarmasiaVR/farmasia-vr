using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpFilterFilter : FilterPart {
    public bool CanBeCut { get => Attached && ConnectedItem == null; }
}
