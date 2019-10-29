using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    #region Fields
    private HashSet<GameObject> grabObjects;
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

        ObjectHighlight hObject = ObjectHighlight.GetHighlightFromTransform(coll.transform);
        hObject?.StartCoroutine(hObject?.InsideCheck(this));
    }

    private void OnTriggerExit(Collider coll) {
        GameObject interactable = Interactable.GetInteractableObject(coll.transform);
        if (interactable == null) {
            return;
        }
        grabObjects.Remove(interactable);
    }

    public bool Contains(GameObject obj) {
        return grabObjects.Contains(obj);
    }

    public Interactable GetGrabbedInteractable() {
        return GetGrabbedObject()?.GetComponent<Interactable>() ?? null;
    }

    public GameObject GetGrabbedObject() {
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
}
