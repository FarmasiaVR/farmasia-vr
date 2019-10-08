using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour {

    #region Fields
    public GameObject CopyObject { get; set; }
    private GameObject latestCopy;
    private Interactable interactable;
    #endregion

    private void Start() {
        CreateNewCopy();
    }

    private void Update() {
        if (interactable.State == InteractState.Grabbed) {
            latestCopy.GetComponent<Rigidbody>().isKinematic = false;
            CreateNewCopy();
        }
    }

    private void CreateNewCopy() {
        if (latestCopy != null) {
            if (latestCopy.GetComponent<Rigidbody>().isKinematic == true) Destroy(latestCopy);
        }

        latestCopy = Instantiate(CopyObject);
        interactable = latestCopy;
        latestCopy.SetActive(true);
    }
}
