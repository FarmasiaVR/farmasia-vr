using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class TriggerInteractableContainer : MonoBehaviour {
    public Dictionary<Interactable, int> EnteredObjects { get; private set; }

    public delegate void InteractableContainerCallback(Interactable interactable);

    public InteractableContainerCallback OnEnter { get; set; }
    public InteractableContainerCallback OnExit { get; set; }

    protected void Awake() {
        ResetContainer();
    }

    public virtual void ResetContainer() {
        EnteredObjects = new Dictionary<Interactable, int>();
    }

    public Dictionary<Interactable, int>.KeyCollection Objects {
        get {
            return EnteredObjects.Keys;
        }
    }

    public bool Contains(Interactable interactable) {
        if (interactable == null) {
            return false;
        }
        return EnteredObjects.ContainsKey(interactable);
    }

    protected void OnTriggerEnter(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            if (AddToDictionary(i)) {
                OnEnter?.Invoke(i);
            }
        }
    }

    protected void OnTriggerExit(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            if(RemoveFromDictionary(i)) {
                OnExit?.Invoke(i);
            }
        }
    }

    protected bool AddToDictionary(Interactable interactable) {
        if (EnteredObjects.ContainsKey(interactable)) {
            EnteredObjects[interactable]++;
            return false;
        } else {
            EnteredObjects.Add(interactable, 1);
            return true;
        }
    }
    protected bool RemoveFromDictionary(Interactable interactable) {
        if (EnteredObjects.ContainsKey(interactable)) {
            EnteredObjects[interactable]--;

            if (EnteredObjects[interactable] == 0) {
                EnteredObjects.Remove(interactable);
                return true;
            }
        } else {
            Logger.Warning("Object exited invalid amount of times: " + interactable.name);
        }

        return false;
    }
}
