using System;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughCabinet : MonoBehaviour {

    #region fields
    public List<GameObject> objectsInsideArea;
    public Dictionary<String, int> missingObjects;
    #endregion
    
    // Start is called before the first frame update
    void Start() {
        objectsInsideArea = new List<GameObject>();
        missingObjects = new Dictionary<String, int>();
        missingObjects.Add("Needles", 7);
        missingObjects.Add("Big syringe", 1);
        missingObjects.Add("Small syringes", 6);
        missingObjects.Add("Luerlock", 1);
        missingObjects.Add("Bottle", 1);
    }

    private void OnTriggerEnter(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        if (foundObject?.GetComponent<GeneralItem>() == null) {
            return;
        }
        
        if (!objectsInsideArea.Contains(foundObject)) {
            objectsInsideArea.Add(foundObject);
            ObjectType type = foundObject.GetComponent<GeneralItem>().ObjectType;
            String itemType = Enum.GetName(type.GetType(), type);
            
            if (itemType == "Syringe") {
                Syringe syringe = foundObject.GetComponent<GeneralItem>() as Syringe;
                if (syringe.Container.Capacity == 20) {
                    itemType = "Big syringe";
                } else if (syringe.Container.Capacity == 1) {
                    itemType = "Small syringes";
                }
            }
            if (itemType == "Needle") {
                itemType = "Needles";
            }

            if (missingObjects.ContainsKey(itemType) && (missingObjects[itemType] > 0)) {
                missingObjects[itemType]--; 
            }      
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        if (foundObject?.GetComponent<GeneralItem>() == null) {
            return;
        }
        objectsInsideArea.Remove(foundObject);
        ObjectType type = foundObject.GetComponent<GeneralItem>().ObjectType;
        String itemType = Enum.GetName(type.GetType(), type);
        
        if (itemType == "Syringe") {
            Syringe syringe = foundObject.GetComponent<GeneralItem>() as Syringe;
            if (syringe.Container.Capacity == 20) {
                itemType = "Big syringe";
            } else if (syringe.Container.Capacity == 1) {
                itemType = "Small syringes";
            }
        }
        if (itemType == "Needle") {
            itemType = "Needles";
        }

        if (missingObjects.ContainsKey(itemType)) {
            switch (itemType) {
                case "Needles":
                    if (missingObjects[itemType] < 7) {
                        missingObjects[itemType]++;
                    }
                    break;
                case "Big syringe":
                    if (missingObjects[itemType] == 0) {
                       missingObjects[itemType]++; 
                    }
                    break;
                case "Small syringes":
                    if (missingObjects[itemType] < 6) {
                       missingObjects[itemType]++; 
                    }
                    break;
                case "Luerlock":
                    if (missingObjects[itemType] == 0) {
                       missingObjects[itemType]++; 
                    }
                    break;
                case "Bottle": 
                    if (missingObjects[itemType] == 0) {
                       missingObjects[itemType]++; 
                    }       
                    break;
            }
        }  
    }

    /// <summary>
    /// Returns list presentation of contained gameobjects.
    /// </summary>
    public List<GameObject> GetContainedItems() {
        return objectsInsideArea;
    }

    public String GetMissingItems() {
        String missing = "Missing items:";
        foreach(KeyValuePair<String, int> value in missingObjects) {
            if (value.Value > 0) {
                missing = missing + " " + value.Key + " " + value.Value + ",";    
            } 
        }
        return missing;
    }

    // Update is called once per frame
    void Update() { 
    }
}
