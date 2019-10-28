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
        /* DEMO missingObjects.Add("Needles", 7);*/
        missingObjects.Add("Big syringe", 1);
        missingObjects.Add("Small syringes", 6);
        missingObjects.Add("Luerlock", 1);
        missingObjects.Add("Bottle", 1);

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

            if (itemType == "Syringe") {
                Syringe syringe = item as Syringe;
                if (syringe.Container.Capacity == 5000) {
                    itemType = "Big syringe";
                } else if (syringe.Container.Capacity == 1000) {
                    itemType = "Small syringes";
                }
            }
            /* DEMO if (itemType == "Needle") {
                itemType = "Needles";
            }*/

            else if (itemType == "Bottle") {
                MedicineBottle bottle = item as MedicineBottle;
                if (bottle.Container.Capacity != 80000) {
                    return;
                }
            }

            if (missingObjects.ContainsKey(itemType) && missingObjects[itemType] > 0) {
                missingObjects[itemType]--;
            }
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

        if (itemType == "Syringe") {
            Syringe syringe = item as Syringe;
            if (syringe.Container.Capacity == 5000) {
                itemType = "Big syringe";
            } else if (syringe.Container.Capacity == 1000) {
                itemType = "Small syringes";
            }
        } else if (itemType == "Bottle") {
            MedicineBottle bottle = item as MedicineBottle;
            if (bottle.Container.Capacity != 80000) {
                return;
            }
        }

        /* DEMO if (itemType == "Needle") {
            itemType = "Needles";
        }*/

        if (missingObjects.ContainsKey(itemType)) {
            switch (itemType) {
                /* DEMO case "Needles":
                    if (missingObjects[itemType] < 7) {
                        missingObjects[itemType]++;
                    }
                    break;*/
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
        foreach (KeyValuePair<String, int> value in missingObjects) {
            if (value.Value > 0) {
                missing = missing + " " + value.Key + " " + value.Value + ",";
            }
        }
        return missing;
    }
}