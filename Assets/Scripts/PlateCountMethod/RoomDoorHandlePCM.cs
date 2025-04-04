using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomDoorHandlePCM : Interactable
{
    public UnityEvent onHandleActivated;
    private Animator animator;

    protected override void Start() {
        base.Start();
        animator = transform.GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        onHandleActivated?.Invoke();
        animator.SetTrigger("TouchDoor");
    }
}
