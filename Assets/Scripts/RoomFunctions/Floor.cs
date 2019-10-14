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
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject foundObject = other.transform.gameObject;
        if (foundObject.GetComponent<GeneralItem>() == null) {
            return;
        }
        objectsOnArea.Remove(foundObject);
        Events.FireEvent(EventType.CleanUp, CallbackData.Boolean(false));
    }

    // Update is called once per frame
    void Update() { 
    }
}