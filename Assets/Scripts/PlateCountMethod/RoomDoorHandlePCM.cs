using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomDoorHandlePCM : Interactable
{
    public UnityEvent onHandleActivated;
    private Animator animator;
    private AudioSource audioSource;

    protected override void Start() {
        base.Start();
        animator = transform.GetComponentInChildren<Animator>();
        audioSource = transform.GetComponent<AudioSource>();
    }

    public void Interact()
    {
        onHandleActivated?.Invoke();
        animator.SetTrigger("TouchDoor");
        audioSource.Play();
    }
}
