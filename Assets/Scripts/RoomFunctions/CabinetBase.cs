using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetBase : MonoBehaviour {

    public enum Types {
        Null,
        Neula,
        IsoRuisku,
        PienetRuiskut,
        Luerlock,
        Lääkepullo,
        KorkkiPussi
    }

    #region Fields
    public enum CabinetType { PassThrough, Laminar }
    [SerializeField]
    public CabinetType type;
    public List<GameObject> objectsInsideArea;
    private Dictionary<Types, int> missingObjects;
    private bool itemPlaced = false;
    [SerializeField]
    private GameObject childCollider;

    
    [SerializeField]
    [Tooltip("Used only in laminar cabinet. This factory will be set active when a SyringeCapBag has entered the laminar cabinet.")]
    private GameObject syringeCapFactory;

    private Pipeline capBagEnterPipeline;
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
        missingObjects.Add(Types.KorkkiPussi, 1);

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterCabinet(collider)));
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnExit(collider => ExitCabinet(collider)));
        syringeCapFactory.SetActive(false);
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
            Types underlyingType = CheckItemType(type, item, enteringCabinet: true);
            missingObjects[underlyingType]--;
        }
    }

    private void ExitCabinet(Collider other) {
        if (other?.transform.parent?.gameObject.GetComponent<Hand>() != null && this.type == CabinetType.Laminar) {
            Events.FireEvent(EventType.HandsExitLaminarCabinet, CallbackData.NoData());
        }
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        objectsInsideArea.Remove(foundObject);
        ObjectType type = item.ObjectType;
        Types underlyingType = CheckItemType(type, item, enteringCabinet: false);
        ReAddMissingObjects(underlyingType);
    }

    private Types CheckItemType(ObjectType itemType, GeneralItem item, bool enteringCabinet) {
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
        } else if (itemType == ObjectType.SyringeCapBag) {
            type = Types.KorkkiPussi;
            if (this.type == CabinetType.Laminar && enteringCabinet) {
                SyringeCapBagEnteredLaminarCabinet(item);
            }
        }
        return type;
    }

    private void SyringeCapBagEnteredLaminarCabinet(GeneralItem capBag) {
        capBagEnterPipeline = G.Instance.Pipeline
                                    .New()
                                    .Delay(1.5f)
                                    .TFunc(DestroyCapBagAndSetFactoryActive, () => capBag);
    }

    private void DestroyCapBagAndSetFactoryActive(GeneralItem capBag) {
        if (capBag != null && objectsInsideArea.Contains(capBag.gameObject)) {
            
            Logger.Print("Syringe cap bag still inside cabinet, destroying bag and setting factory active...");

            if (!capBag.IsClean) {
                Logger.Print("Setting cap factory not clean");
                syringeCapFactory.GetComponent<GeneralItem>().IsClean = false;
            }
            syringeCapFactory.SetActive(true);
  
            capBag.DestroyInteractable();
        } else {
            Logger.Print("Syringe cap bag not inside cabinet anymore, won't destroy or set factory active");
        }
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