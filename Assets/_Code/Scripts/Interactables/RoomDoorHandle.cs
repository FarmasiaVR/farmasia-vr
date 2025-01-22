
using UnityEngine;

public class RoomDoorHandle : AnimatedDoorHandle {

    public DoorGoTo destination;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        if (destination == DoorGoTo.None) {
            return;
        }
        Events.FireEvent(EventType.RoomDoor, CallbackData.Object(destination));
    }

    public void Interact() {
        Debug.Log("trying to enter room");
        if (destination == DoorGoTo.None) {
            return;
        }
        Debug.Log("trying to enter room after check of destination");
        Events.FireEvent(EventType.RoomDoor, CallbackData.Object(destination));
    }

    public void resetCounterForPlates()
    {
        Events.FireEvent(EventType.ResetCounter);
    }


}
