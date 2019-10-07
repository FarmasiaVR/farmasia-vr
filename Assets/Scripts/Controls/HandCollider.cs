using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    #region fields
    private Hand hand;
    private static string iTag = "Interactable";
    public HashSet<GameObject> GrabObjects { get; private set; }
    #endregion

    private void Start() {
        GrabObjects = new HashSet<GameObject>();
    }

    public Hand Hand {
        get {
            if (hand == null) {
                hand = transform.parent.GetComponent<Hand>();
            }
            return hand;
        }
    }

    public void OnTriggerEnter(Collider coll) {

        GameObject interactable = Interactable.GetInteractableObject(coll.transform);

        if (interactable == null) {
            return;
        }


        GrabObjects.Add(interactable);

        ObjectHighlight hObject = ObjectHighlight.GetHighlightFromTransform(coll.transform);
        if (hObject == null) {
            return;
        }
        hObject.StartCoroutine(hObject.InsideCheck(this));
    }
    public void OnTriggerExit(Collider coll) {

        GameObject interactable = Interactable.GetInteractableObject(coll.transform);

        if (interactable == null) {
            return;
        }

        GrabObjects.Remove(interactable);
    }

    


    public bool Contains(GameObject obj) {
        return GrabObjects.Contains(obj);
    }

    public Interactable GetGrab() {
        GameObject o = GetGrabObject();
        return o?.GetComponent<Interactable>() ?? null;
    }

    public GameObject GetGrabObject() {
        float closestDistance = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject rb in GrabObjects) {
            float distance = Vector3.Distance(transform.position, rb.transform.position);

            if (distance < closestDistance) {
                closestDistance = distance;
                closest = rb;
            }
        }

        return closest;
    }
}
