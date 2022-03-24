using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class TriggerInteractableContainer : MonoBehaviour {
    public bool IsHandCollider;
    public Dictionary<Interactable, int> EnteredObjects { get; private set; }
    public Dictionary<Interactable, (Vector3, float, Collider)> EnteredObjectPoints { get; private set; }

    public delegate void InteractableContainerCallback(Interactable interactable);

    public InteractableContainerCallback OnEnter { get; set; }
    public InteractableContainerCallback OnExit { get; set; }

    private void Awake() {
        ResetContainer();
    }

    public void ResetContainer() {
        EnteredObjects = new Dictionary<Interactable, int>();
        EnteredObjectPoints = new Dictionary<Interactable, (Vector3, float, Collider)>();
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

    private void OnTriggerEnter(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            if (AddToDictionary(i)) {
                OnEnter?.Invoke(i);
            }

            if (other.isTrigger && !(other.gameObject.layer == 5)) return;
            if (!IsHandCollider) return;


            Vector3 newPosition = other.ClosestPoint(transform.position);
            float newDistance = Vector3.Distance(transform.position, newPosition);
            if (!(EnteredObjectPoints.ContainsKey(i))) {
                EnteredObjectPoints.Add(i, (newPosition, newDistance, other));
            } else if (EnteredObjectPoints[i].Item2 > newDistance) {
                EnteredObjectPoints[i] = (newPosition, newDistance, other);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.isTrigger && !(other.gameObject.layer == 5)) return;

        if (!IsHandCollider) return;

        Vector3 newPosition = other.ClosestPoint(transform.position);
        float newDistance = Vector3.Distance(transform.position, newPosition);
        if (Interactable.GetInteractable(other.transform) is var i && i != null && EnteredObjectPoints.ContainsKey(i) && EnteredObjectPoints[i].Item2 > newDistance) {
            EnteredObjectPoints[i] = (newPosition, newDistance, other);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            if(RemoveFromDictionary(i)) {
                OnExit?.Invoke(i);
            }

            if (other.isTrigger && !(other.gameObject.layer == 5)) return;

            if (!IsHandCollider) return;

            //TODO: Fix edge cases!
            if (!EnteredObjectPoints.ContainsKey(i)) return;
            if (EnteredObjectPoints[i].Item3 == other) {
                EnteredObjectPoints.Remove(i);
            }

        }
    }

    private bool AddToDictionary(Interactable interactable) {
        if (EnteredObjects.ContainsKey(interactable)) {
            EnteredObjects[interactable]++;
            return false;
        } else {
            EnteredObjects.Add(interactable, 1);
            return true;
        }
    }
    private bool RemoveFromDictionary(Interactable interactable) {
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
