using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    #region Fields
    private HashSet<GameObject> grabObjects;
    public ObjectHighlight PreviousHighlight { get; private set; }
    #endregion

    private void Start() {
        grabObjects = new HashSet<GameObject>();
    }

    private void OnTriggerEnter(Collider coll) {
        GameObject interactable = Interactable.GetInteractableObject(coll.transform);
        if (interactable != null) {
            grabObjects.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider coll) {
        GameObject interactable = Interactable.GetInteractableObject(coll.transform);
        if (interactable == null) {
            return;
        }

        ObjectHighlight highlight = ObjectHighlight.GetHighlightFromTransform(coll.transform);
        if (highlight == PreviousHighlight) UnhighlightPrevious();
        grabObjects.Remove(interactable);
    }

    #region Highlight
    public void HighlightClosestObject() {
        HighlightObject(GetClosestObject());
    }

    public void HighlightPointedObject(float maxAngle) {
        HighlightObject(GetPointedObject(maxAngle));
    }

    public void UnhighlightAll() {
        foreach (GameObject child in grabObjects) {
            ObjectHighlight.GetHighlightFromTransform(child.transform)?.Unhighlight();
        }
    }

    private void HighlightObject(GameObject obj) {
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

    public GameObject GetClosestObject() {
        float closestDistance = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject rb in grabObjects) {
            float distance = Vector3.Distance(transform.position, rb.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = rb;
            }
        }

        return closest;
    }

    public GameObject GetPointedObject(float maxAngle) {
        float smallestAngle = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject g in grabObjects) {
            float angle = Vector3.Angle(transform.forward, g.transform.position - transform.position);

            if (angle < smallestAngle && angle < maxAngle) {
                smallestAngle = angle;
                closest = g;
            }
        }

        return closest;
    }
}
