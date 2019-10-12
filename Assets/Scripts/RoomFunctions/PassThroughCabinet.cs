using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughCabinet : MonoBehaviour {
    public List<GameObject> objectsInsideArea;
    public List<String> missingObjects;
    // Start is called before the first frame update
    void Start() {
        objectsInsideArea = new List<GameObject>();
        missingObjects = new List<String>() {"Needle", "Luerlock", "Bottle", "Syringe"};
    }

    private void OnTriggerEnter(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        
        if (!objectsInsideArea.Contains(foundObject)) {
            objectsInsideArea.Add(foundObject);
            ObjectType type = foundObject.GetComponent<GeneralItem>().ObjectType;
            String itemType = Enum.GetName(type.GetType(), type);
            if (missingObjects.Contains(itemType)) {
                missingObjects.Remove(itemType);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        objectsInsideArea.Remove(foundObject);
        ObjectType type = foundObject.GetComponent<GeneralItem>().ObjectType;
        String itemType = Enum.GetName(type.GetType(), type);
        if (!missingObjects.Contains(itemType)) {
            missingObjects.Add(itemType);
        } 
    }

    public String GetMissingItems() {
        String missing = "Missing items:";
        foreach(String value in missingObjects) {
            missing = missing + " " + value + ",";
        }
        return missing;
    }

    // Update is called once per frame
    void Update() { 
    }
}
