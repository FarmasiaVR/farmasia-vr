using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    private static string iTag = "Interactable";
    public List<GameObject> grabObjects;

    public void OnTriggerEnter(Collider coll) {

        if (coll.gameObject.tag != iTag) {
            return;
        }

        ObjectHighlight hObject = coll.gameObject.GetComponent<ObjectHighlight>();

        hObject.StartCoroutine(hObject.InsideCheck(this));

        grabObjects.Add(coll.gameObject);
    }
    public void OnTriggerExit(Collider coll) {

        if (coll.gameObject.tag != iTag) {
            return;
        }

        grabObjects.Remove(coll.gameObject);
    }

    public bool Contains(GameObject obj) {
        return grabObjects.Contains(obj);
    }
    public List<GameObject> getGrabList() {
        return grabObjects;
    }

    public Interactable GetGrab() {

        GameObject o = GetGrabObject();

        if (o == null) {
            return null;
        }

        return GetGrabObject().GetComponent<Interactable>();
    }

    public GameObject GetGrabObject() {

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
