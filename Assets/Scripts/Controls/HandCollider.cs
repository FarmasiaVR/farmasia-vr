using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    #region Fields
    private HashSet<Interactable> grabObjects;
    public ObjectHighlight PreviousHighlight { get; private set; }
    #endregion

    private void Start() {
        grabObjects = new HashSet<Interactable>();
    }

    private void OnTriggerEnter(Collider coll) {
        Interactable interactable = Interactable.GetInteractable(coll.transform);
        if (interactable != null) {
            grabObjects.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider coll) {
        Interactable interactable = Interactable.GetInteractable(coll.transform);
        if (interactable == null) {
            return;
        }

        ObjectHighlight highlight = ObjectHighlight.GetHighlightFromTransform(coll.transform);
        if (highlight == PreviousHighlight) UnhighlightPrevious();
        grabObjects.Remove(interactable);
    }

    public void RemoveInteractable(Interactable interactable) {
        if (grabObjects.Contains(interactable)) {
            grabObjects.Remove(interactable);
        }
    }

    #region Highlight
    public void HighlightClosestObject() {
        HighlightObject(GetClosestObject());
    }

    public void HighlightPointedObject(float maxAngle) {
        HighlightObject(GetPointedObject(maxAngle));
    }

    public void UnhighlightAll() {
        foreach (Interactable child in grabObjects) {
            ObjectHighlight.GetHighlightFromTransform(child.transform)?.Unhighlight();
        }
    }

    private void HighlightObject(Interactable obj) {
        UnhighlightPrevious();

        if (grabObjects.Contains(obj)) {
            PreviousHighlight = ObjectHighlight.GetHighlightFromTransform(obj.transform);
            PreviousHighlight?.Highlight();
        }
    }
    #endregion

    public void UnhighlightPrevious() {
        PreviousHighlight?.Unhighlight();
        PreviousHighlight = null;
    }

    public Interactable GetClosestInteractable() {
        return GetClosestObject()?.GetComponent<Interactable>() ?? null;
    }

    public Interactable GetClosestObject() {
        float closestDistance = float.MaxValue;
        Interactable closest = null;

        foreach (Interactable rb in grabObjects) {
            float distance = Vector3.Distance(transform.position, rb.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = rb;
            }
        }

        return closest;
    }

    public Interactable GetPointedObject(float maxAngle) {
        float smallestAngle = float.MaxValue;
        Interactable closest = null;

        foreach (Interactable g in grabObjects) {
            float angle = Vector3.Angle(transform.forward, g.transform.position - transform.position);

            if (angle < smallestAngle && angle < maxAngle) {
                smallestAngle = angle;
                closest = g;
            }
        }

        return closest;
    }
}
