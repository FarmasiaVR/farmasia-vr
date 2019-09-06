using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    public List<Rigidbody> grabObjects;


    public Rigidbody GetGrabObjet() {

        float closestDistance = float.MaxValue;
        Rigidbody closest = null;

        foreach (Rigidbody rb in grabObjects) {

            float distance = Vector3.Distance(transform.position, rb.transform.position);

            if (distance < closestDistance) {
                closestDistance = distance;
                closest = rb;
            }
        }

        return closest;
    }

    public void OnTriggerEnter(Collider coll) {

        if (coll.gameObject.tag != "Grabbable") {
            return;
        }

        grabObjects.Add(coll.GetComponent<Rigidbody>());
    }
    public void OnTriggerExit(Collider coll) {

        if (coll.gameObject.tag != "Grabbable") {
            return;
        }

        grabObjects.Remove(coll.GetComponent<Rigidbody>());
    }
}
