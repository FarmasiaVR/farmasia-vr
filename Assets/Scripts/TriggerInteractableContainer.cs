using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractableContainer : MonoBehaviour {

    private HashSet<Interactable> interactables;

    private void Awake() {
        interactables = new HashSet<Interactable>();
    }

    public bool Contains(Interactable interactable) {
        return interactables.Contains(interactable);
    }

    private void OnTriggerEnter(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            interactables.Add(i);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            interactables.Remove(i);
        }
    }
}
