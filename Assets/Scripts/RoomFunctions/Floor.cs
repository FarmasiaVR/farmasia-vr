using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    public List<GameObject> objectsOnArea;
    // Start is called before the first frame update
    void Start() {
        objectsOnArea = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        
        if (!objectsOnArea.Contains(foundObject)) {
            objectsOnArea.Add(foundObject);
            ObjectType type = foundObject.GetComponent<GeneralItem>().ObjectType;
            String itemType = Enum.GetName(type.GetType(), type);      
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        objectsOnArea.Remove(foundObject);
        ObjectType type = foundObject.GetComponent<GeneralItem>().ObjectType;
        String itemType = Enum.GetName(type.GetType(), type);  
    }

    // Update is called once per frame
    void Update() { 
    }
}