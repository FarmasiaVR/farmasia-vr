using UnityEngine;
using UnityEngine.Assertions;

public class Pump : ReceiverItem
{
    protected override void ConnectAttachment() {
        base.ConnectAttachment();

        Events.FireEvent(EventType.AttachFilter, CallbackData.Object(this));
    }
}
