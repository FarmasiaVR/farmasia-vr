using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterPart : ReceiverItem
{
    public LiquidContainer Container { get; private set; }

    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);
    }

}
