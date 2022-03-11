using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPipette : ReceiverItem
{
    public BigPipette() {
        AfterRelease = (interactable) => { Logger.Print("Pipette container released"); };
    }
}
