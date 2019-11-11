using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    #region Fields
    private HashSet<GameObject> grabObjects;

    [SerializeField]
    private bool useHighlighting;
    #endregion

    private void Start() {
        grabObjects = new HashSet<GameObject>();
    }

    private void OnTriggerEnter(Collider coll) {
        GameObject interactable = Interactable.GetInteractableObject(coll.transform);
        if (interactable == null) {
            return;
        }
        grabObjects.Add(interactable);

        if (useHighlighting) {
            ObjectHighlight hObject = ObjectHighlight.GetHighlightFromTransform(coll.transform);
            if (hObject != null) {
                StartCoroutine(InsideCheck(coll.gameObject, hObject));
            }
        }

        IEnumerator InsideCheck(Interactable obj, ObjectHighlight highlight) {
            while (grabObjects.Contains(obj.gameObject)) {
                if (ShouldHighlight(obj)) {
                    highlight.Highlight();
                } else {
                    highlight.Unhighlight();
                }

                yield return null;
            }

            highlight.Unhighlight();
        }
    }

    private void OnTriggerExit(Collider coll) {
        GameObject interactable = Interactable.GetInteractableObject(coll.transform);
        if (interactable == null) {
            return;
        }
        grabObjects.Remove(interactable);
    }

    public Interactable GetClosestInteractable() {
        return GetClosestObject()?.GetComponent<Interactable>() ?? null;
    }

    private GameObject GetClosestObject() {
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

    private bool ShouldHighlight(Interactable interactable) {
        if (interactable.State == InteractState.Grabbed) {
            return false;
        }
        
        return interactable.gameObject == GetClosestObject();
    }

    public int CountWithinAngle(float maxAngle) {
        return GetObjectsWithinAngle(maxAngle).Count;
    }

    public GameObject GetPointedObject(float maxAngle) {

        List<GameObject> objects = GetObjectsWithinAngle(maxAngle);

        float smallestAngle = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject g in objects) {

            float angle = Vector3.Angle(transform.forward, g.transform.position - transform.position);

            if (angle < smallestAngle) {
                smallestAngle = angle;
                closest = g;
            }
        }

        return closest;
    }

    private List<GameObject> GetObjectsWithinAngle(float maxAngle) {

        List<GameObject> list = new List<GameObject>();

        foreach (GameObject g in grabObjects) {
            float angle = Vector3.Angle(transform.forward, g.transform.position - transform.position);

            if (angle <= maxAngle) {
                list.Add(g);
            }
        }

        return list;
    }
}
