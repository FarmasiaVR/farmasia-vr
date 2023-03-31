using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 This script allows disabling collisions between the colliders of the object in the scoket and the colliders of the object that owns the socket

    EnableColliders and DisableColliders are called by the socket interactor select event, which is triggered when a interactable is attached or detached from a socket
*/
public class DisableAttachedObjectCollider : MonoBehaviour
{
    public XRGrabInteractable socketsOwnerInteractable;
    private XRSocketInteractor socket;

    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        if (socket.startingSelectedInteractable)
        {
            SelectEnterEventArgs args = new SelectEnterEventArgs();
            args.interactableObject  = socket.startingSelectedInteractable;
            DisableCollisions(args);
        }
    }

    //sets the list of colliders to ignore the colliders of the socketOwner
    public void setCollidersIgnoreState(bool shouldIgnoreCollisions, List<Collider> otherColliders) {
        foreach (Collider ownersColl in socketsOwnerInteractable.colliders) {
            foreach (Collider attachedColl in otherColliders) {
                Physics.IgnoreCollision(ownersColl, attachedColl, shouldIgnoreCollisions);
            }
        }
    }

    //called when interactable is detached from a socket
    public void EnableCollisions(SelectExitEventArgs args) {
        setCollidersIgnoreState(false, args.interactableObject.colliders);
    }

    //Called when interactable is attached to a socket
    public void DisableCollisions(SelectEnterEventArgs args) {
        setCollidersIgnoreState(true, args.interactableObject.colliders);
    }

}
