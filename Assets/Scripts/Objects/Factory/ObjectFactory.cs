using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour {

    #region Fields
    public GameObject CopyObject { get; set; }
    public GameObject LatestCopy { get; set; }
    private Interactable interactable;

    private GameObject lastPicked;

    public bool IsEnabled { get; set; } = true;

    public ColliderHitCount triggerColliderCount;
    #endregion

    private void Start() {
        CreateColliderCopy();
        CreateNewCopy();
    }


    #region Collider initialiazation

    private void CreateColliderCopy() {

        GameObject triggerCopy = new GameObject();

        CopyTransform(CopyObject.transform, triggerCopy.transform);



        triggerColliderCount = triggerCopy.AddComponent<ColliderHitCount>();
        CopyObject.SetActive(true);
        RecursiveCopyColliders(CopyObject.transform, triggerCopy.transform);
        CopyObject.SetActive(false);
    }

    private void RecursiveCopyColliders(Transform original, Transform copy) {

        Collider coll = original.GetComponent<Collider>();
        if (coll != null) {
            Vector3 size = coll.bounds.size;

            //Logger.PrintVariables("coll size", coll.bounds.size, "coll extents", coll.bounds.extents);

            AddCollider(copy.gameObject, size);
            // ComponentCopy.Copy<Collider>(coll, copy.gameObject).isTrigger = true;
            triggerColliderCount.SubscribeToCollisions(copy.gameObject);
        }


        foreach (Transform originalChild in original) {

            GameObject copyChild = new GameObject();
            copyChild.transform.parent = copy.transform;
            CopyTransform(originalChild, copyChild.transform);

            RecursiveCopyColliders(originalChild, copyChild.transform);
        }
    }
    private void AddCollider(GameObject g, Vector3 size) {

        BoxCollider coll = g.AddComponent<BoxCollider>();

        coll.size = size;
        coll.isTrigger = true;
    }


    private void CopyTransform(Transform from, Transform to) {
        to.transform.position = from.transform.position;
        to.transform.localRotation = from.transform.localRotation;
        // to.transform.localScale = from.transform.localScale;
        to.transform.name = from.transform.name + " (TriggerCopy)";
    }
    #endregion

    private void Update() {
        if (interactable.State == InteractState.Grabbed && IsEnabled) {
            LatestCopy.GetComponent<Rigidbody>().isKinematic = false;
            CreateNewCopy();
        }
    }

    private IEnumerator CheckCollisionRelease(GameObject handObject, GameObject factoryObject, Interactable currentInteractable) {
        yield return null;

        Interactable handInteractable = Interactable.GetInteractable(handObject.transform);
        
        while (MultiColliderTool.CheckCollision(handObject, factoryObject)) {
            if (currentInteractable != interactable) {
                handInteractable.DestroyInteractable();
                yield break;
            }

            if (!handInteractable.IsGrabbed) {
                handInteractable.DestroyInteractable();
                yield break;
            }

            yield return null;
        }

        CollisionIgnore.IgnoreCollisions(handObject.transform, factoryObject.transform, false);
    }

    private void CreateNewCopy() {
        GameObject handObject = LatestCopy;

        if (LatestCopy != null) {
            triggerColliderCount.SetInteractable(handObject);

            if (LatestCopy.GetComponent<Rigidbody>().isKinematic == true) {
                Destroy(LatestCopy);
            }
        }

        LatestCopy = Instantiate(CopyObject);
        LatestCopy.transform.position = transform.position;

        interactable = LatestCopy.GetComponent<Interactable>();
        LatestCopy.SetActive(true);

        if (handObject != null) {
            CollisionIgnore.IgnoreCollisions(handObject.transform, LatestCopy.transform, true);
            StartCoroutine(CheckCollisionRelease(handObject, LatestCopy, interactable));
        }

        lastPicked = handObject;
    }
}
