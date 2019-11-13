using System;
using System.Collections.Generic;
using UnityEngine;

public class CabinetBase : MonoBehaviour {

    public enum Types {
        Null,
        Neula,
        IsoRuisku,
        PienetRuiskut,
        Luerlock,
        Lääkepullo
    }

    #region fields
    public enum CabinetType { PassThrough, Laminar }
    [SerializeField]
    public CabinetType type;
    public List<GameObject> objectsInsideArea;
    private Dictionary<Types, int> missingObjects;
    private bool itemPlaced = false;
    [SerializeField]
    private GameObject childCollider;
    #endregion

    // Start is called before the first frame update
    void Start() {
        objectsInsideArea = new List<GameObject>();
        missingObjects = new Dictionary<Types, int>();
        missingObjects.Add(Types.Neula, 1);
        missingObjects.Add(Types.IsoRuisku, 1);
        missingObjects.Add(Types.PienetRuiskut, 6);
        missingObjects.Add(Types.Luerlock, 1);
        missingObjects.Add(Types.Lääkepullo, 1);

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterCabinet(collider)));
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnExit(collider => ExitCabinet(collider)));
    }

    private void EnterCabinet(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            Logger.Print(other.gameObject.name);
            return;
        }
        if (!itemPlaced) {
            Events.FireEvent(EventType.ItemPlacedInCabinet, CallbackData.Object(this));
            itemPlaced = true;
        }
        if (!objectsInsideArea.Contains(foundObject)) {
            objectsInsideArea.Add(foundObject);
            ObjectType type = item.ObjectType;
            Types underlyingType = CheckItemType(type, item);
            missingObjects[underlyingType]--;
        }
    }

    private void ExitCabinet(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        if (foundObject?.GetComponent<Hand>() != null && this.type == CabinetType.Laminar) {
            Events.FireEvent(EventType.HandsExitLaminarCabinet, CallbackData.NoData());
        }
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        objectsInsideArea.Remove(foundObject);
        ObjectType type = item.ObjectType;
        Types underlyingType = CheckItemType(type, item);
        ReAddMissingObjects(underlyingType);
    }

    private Types CheckItemType(ObjectType itemType, GeneralItem item) {
        Types type = Types.Null;
        if (itemType == ObjectType.Syringe) {
            Syringe syringe = item as Syringe;
            if (syringe.Container.Capacity == 20000) {
                type = Types.IsoRuisku;
            } else if (syringe.Container.Capacity == 1000) {
                type = Types.PienetRuiskut;
            }
        } else if (itemType == ObjectType.Bottle) {
            type = Types.Lääkepullo;
        } else if (itemType == ObjectType.Needle) {
            type = Types.Neula;
        } else if (itemType == ObjectType.Luerlock) {
            type = Types.Luerlock;
        }
        return type;
    }

    private void ReAddMissingObjects(Types itemType) {
        if (missingObjects.ContainsKey(itemType)) {
            switch (itemType) {
                case Types.Neula:
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.IsoRuisku:
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.PienetRuiskut:
                    if (missingObjects[itemType] < 6) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.Luerlock:
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.Lääkepullo:
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
        String missing = "Puuttuvat välineet:";
        foreach (KeyValuePair<Types, int> value in missingObjects) {
            if (value.Value > 0) {
                missing = missing + " " + value.Key + " " + value.Value + " kpl, \n";
            }
        }
        return missing;
    }
}