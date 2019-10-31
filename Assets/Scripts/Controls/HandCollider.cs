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
            hObject?.StartCoroutine(hObject?.InsideCheck(this));
        }
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
