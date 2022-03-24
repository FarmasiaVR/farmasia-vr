using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTriggerInteractableContainer : TriggerInteractableContainer
{
    public Dictionary<Interactable, (Vector3, float, Collider)> EnteredObjectPoints { get; private set; }

    public override void ResetContainer() {
        base.ResetContainer();
        EnteredObjectPoints = new Dictionary<Interactable, (Vector3, float, Collider)>();
    }

    private new void OnTriggerEnter(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            if (AddToDictionary(i)) {
                OnEnter?.Invoke(i);
            }

            if (other.isTrigger && !(other.gameObject.layer == 5)) return;

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

        Vector3 newPosition = other.ClosestPoint(transform.position);
        float newDistance = Vector3.Distance(transform.position, newPosition);
        if (Interactable.GetInteractable(other.transform) is var i && i != null && EnteredObjectPoints.ContainsKey(i) && EnteredObjectPoints[i].Item2 > newDistance) {
            EnteredObjectPoints[i] = (newPosition, newDistance, other);
        }
    }

    private new void OnTriggerExit(Collider other) {
        if (Interactable.GetInteractable(other.transform) is var i && i != null) {
            if (RemoveFromDictionary(i)) {
                OnExit?.Invoke(i);
            }

            if (other.isTrigger && !(other.gameObject.layer == 5)) return;

            //TODO: Fix edge cases!
            if (!EnteredObjectPoints.ContainsKey(i)) return;
            if (EnteredObjectPoints[i].Item3 == other) {
                EnteredObjectPoints.Remove(i);
            }

        }
    }
}
