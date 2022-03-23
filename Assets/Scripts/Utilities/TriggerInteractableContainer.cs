using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractableContainer : MonoBehaviour {
    public bool IsHandCollider;
    public Dictionary<Interactable, int> EnteredObjects { get; private set; }
    public Dictionary<Interactable, Dictionary<Collider, Vector3>> EnteredObjectPoints { get; private set; }

    public delegate void InteractableContainerCallback(Interactable interactable);

    public InteractableContainerCallback OnEnter { get; set; }
    public InteractableContainerCallback OnExit { get; set; }

    private void Awake() {
        ResetContainer();
    }

    public void ResetContainer() {
        EnteredObjects = new Dictionary<Interactable, int>();
        EnteredObjectPoints = new Dictionary<Interactable, Dictionary<Collider, Vector3>>();
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

            if (other.isTrigger || !IsHandCollider) return;

            if (!(EnteredObjectPoints.ContainsKey(i))) {
                EnteredObjectPoints.Add(i, new Dictionary<Collider, Vector3>());
            }

            if (EnteredObjectPoints[i].ContainsKey(other)) {
                EnteredObjectPoints[i][other] = other.ClosestPoint(transform.position);
            } else {
                EnteredObjectPoints[i].Add(other, other.ClosestPoint(transform.position));
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.isTrigger || !IsHandCollider) return;

        if (Interactable.GetInteractable(other.transform) is var i && i != null && EnteredObjectPoints.ContainsKey(i) && EnteredObjectPoints[i].ContainsKey(other)) {
            if (other is MeshCollider mesh && !mesh.convex) Debug.Log(other.transform.parent.name);
            EnteredObjectPoints[i][other] = other.ClosestPoint(transform.position);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            if(RemoveFromDictionary(i)) {
                OnExit?.Invoke(i);
            }

            if (other.isTrigger || !IsHandCollider) return;

            if (!EnteredObjectPoints.ContainsKey(i)) return;
            EnteredObjectPoints[i]?.Remove(other);
            if (EnteredObjectPoints[i].Count == 0) {
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
