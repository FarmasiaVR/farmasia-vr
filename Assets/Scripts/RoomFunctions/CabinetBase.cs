using System;
using System.Collections.Generic;
using UnityEngine;

public class CabinetBase : MonoBehaviour {
    #region fields
    public enum CabinetType { PassThrough, Laminar }
    [SerializeField]
    public CabinetType type;
    public List<GameObject> objectsInsideArea;
    public Dictionary<String, int> missingObjects;
    private bool itemPlaced = false;
    [SerializeField]
    private GameObject childCollider;
    #endregion

    // Start is called before the first frame update
    void Start() {
        objectsInsideArea = new List<GameObject>();
        missingObjects = new Dictionary<String, int>();
        missingObjects.Add("neula", 1);
        missingObjects.Add("20ml ruisku", 1);
        missingObjects.Add("1ml ruiskut", 6);
        missingObjects.Add("luerlock", 1);
        missingObjects.Add("lääkepullo", 1);

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterCabinet(collider)));
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnExit(collider => ExitCabinet(collider)));
    }

    private void EnterCabinet(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        if (!itemPlaced) {
            Events.FireEvent(EventType.ItemPlacedInCabinet, CallbackData.Object(this));
            itemPlaced = true;
        }


        if (!objectsInsideArea.Contains(foundObject)) {
            objectsInsideArea.Add(foundObject);
            ObjectType type = item.ObjectType;
            String itemType = Enum.GetName(type.GetType(), type);
            CheckItemType(itemType, item);
        }
    }

    private void ExitCabinet(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }

        objectsInsideArea.Remove(foundObject);
        ObjectType type = item.ObjectType;
        String itemType = Enum.GetName(type.GetType(), type);
        CheckItemType(itemType, item);

        if (missingObjects.ContainsKey(itemType)) {
            switch (itemType) {
                case "neula":
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case "20ml ruisku":
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case "1ml ruiskut":
                    if (missingObjects[itemType] < 6) {
                        missingObjects[itemType]++;
                    }
                    break;
                case "luerlock":
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case "lääkepullo":
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
            }
        }
    }

    private String CheckItemType(String itemType, GeneralItem item) {
        if (itemType == "Syringe") {
            Syringe syringe = item as Syringe;
            if (syringe.Container.Capacity == 20000) {
                itemType = "20ml ruisku";
            } else if (syringe.Container.Capacity == 1000) {
                itemType = "1ml ruiskut";
            }
        } else if (itemType == "Bottle") {
            itemType = "lääkepullo";
        } else if (itemType == "Needle") {
            itemType = "neula";
        } else if (itemType == "Luerlock") {
            itemType = "luerlock";
        }
        return itemType;
    }

    /// <summary>
    /// Returns list presentation of contained gameobjects.
    /// </summary>
    public List<GameObject> GetContainedItems() {
        return objectsInsideArea;
    }

    public String GetMissingItems() {
        String missing = "Puuttuvat välineet:";
        foreach (KeyValuePair<String, int> value in missingObjects) {
            if (value.Value > 0) {
                missing = missing + " " + value.Key + " " + value.Value + " kpl,";
            }
        }
        return missing;
    }
}