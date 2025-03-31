using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomDoorHandlePCM : Interactable
{
    public UnityEvent onHandleActivated;

    public void Interact()
    {
        onHandleActivated?.Invoke();
    }
}
