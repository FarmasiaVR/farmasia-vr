using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SterileBag : MonoBehaviour {

    #region fields
    public List<GameObject> objectsInBag;
    public bool isClosed;
    public bool isSterile;
    #endregion
    
    // Start is called before the first frame update
    void Start() {
        objectsInBag = new List<GameObject>();
        isClosed = false;
        isSterile = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (isClosed) {
            return;
        }
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        
        if (!objectsInBag.Contains(foundObject)) {
            objectsInBag.Add(foundObject);
            if (!foundObject.GetComponent<GeneralItem>().IsClean) {
                isSterile = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (isClosed) {
            return;
        }
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        objectsInBag.Remove(foundObject);
    }

    // Update is called once per frame
    void Update() { 
    }
}