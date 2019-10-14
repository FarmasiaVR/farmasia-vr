using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour {

    #region fields
    public List<GameObject> objectsInArea;
    public int droppedItemsInArea;
    public bool droppedItemsPutBeforeTime;
    #endregion
    
    void Start() {
        objectsInArea = new List<GameObject>();
        droppedItemsInArea = 0;
        droppedItemsPutBeforeTime = false;
    }

    private void OnTriggerEnter(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        
        if (!objectsInArea.Contains(foundObject)) {
            objectsInArea.Add(foundObject);
            if (!foundObject.GetComponent<GeneralItem>().IsClean) {
                droppedItemsInArea++;
                if (G.Instance.Progress.currentPackage.name != "Clean up") {
                    droppedItemsPutBeforeTime = true;
                    UISystem.Instance.CreatePopup("Dropped item was put to trash before time", MessageType.Notify);
                }
            }     
        }
    }

    void Update() {
        foreach (GameObject obj in objectsInArea) {
            Destroy(obj);
        }    
    }
}