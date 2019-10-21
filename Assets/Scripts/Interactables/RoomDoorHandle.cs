using System;
using UnityEngine;

public class RoomDoorHandle : Interactable {

    #region field
    [SerializeField]
    private DoorGoTo destination;
    #endregion

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        if (destination == DoorGoTo.None) {
            return;
        }
        Events.FireEvent(EventType.RoomDoor, CallbackData.String(destination.ToString()));
    }
}