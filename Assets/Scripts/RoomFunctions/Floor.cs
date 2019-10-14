using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    public List<GameObject> objectsOnArea;
    public int droppedItems;
    // Start is called before the first frame update
    void Start() {
        objectsOnArea = new List<GameObject>();
        droppedItems = 0;
    }

    private void OnTriggerEnter(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        
        if (!objectsOnArea.Contains(foundObject)) {
            objectsOnArea.Add(foundObject);   
            droppedItems++;   
            foundObject.GetComponent<GeneralItem>().IsClean = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        objectsOnArea.Remove(foundObject);
        if (G.Instance.Progress.currentPackage.name != "Clean up") {
            UISystem.Instance.CreatePopup("Dropped items shouldn't be cleaned before finishing other tasks", MessageType.Warning);
        }
    }

    // Update is called once per frame
    void Update() { 
    }
}