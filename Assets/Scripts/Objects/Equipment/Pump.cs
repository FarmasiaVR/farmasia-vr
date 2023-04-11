using UnityEngine;
using UnityEngine.Assertions;

public class Pump : ReceiverItem
{
    protected override void ConnectAttachment() {
        base.ConnectAttachment();

        Events.FireEvent(EventType.AttachFilter, CallbackData.Object(this));
    }

    public void attachFilterXR()

    {
        // Debug.Log("Pump detected filter");
        Events.FireEvent(EventType.AttachFilter, CallbackData.Object(this));
    }
}
