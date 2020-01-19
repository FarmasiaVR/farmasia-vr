using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetBase : MonoBehaviour {

    public enum Types {
        Null,
        Needle,
        BigSyringe,
        SmallSyringe,
        Luerlock,
        MedicineBottle,
        SyringeCapBag
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
    private GameObject sterileDrape;

    [SerializeField]
    [Tooltip("Used only in laminar cabinet. This factory will be set active when a SyringeCapBag has entered the laminar cabinet.")]
    private GameObject syringeCapFactory = null;

    private Pipeline capBagEnterPipeline;
    private bool folded;
    #endregion

    // Start is called before the first frame update
    private void Start() {
        objectsInsideArea = new List<GameObject>();
        missingObjects = new Dictionary<Types, int>();
        missingObjects.Add(Types.Needle, 1);
        missingObjects.Add(Types.BigSyringe, 1);
        missingObjects.Add(Types.SmallSyringe, 6);
        missingObjects.Add(Types.Luerlock, 1);
        missingObjects.Add(Types.MedicineBottle, 1);
        missingObjects.Add(Types.SyringeCapBag, 1);

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterCabinet(collider)));
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnExit(collider => ExitCabinet(collider)));

        if (syringeCapFactory != null) {
            syringeCapFactory.SetActive(false);
        }
    }

    private void EnterCabinet(Collider other) {

        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }

        if (Time.time > 5) {
            UnfoldCloth();
        }

        if (!itemPlaced) {
            Events.FireEvent(EventType.ItemPlacedInCabinet, CallbackData.Object(this));
            itemPlaced = true;
        }
        if (!objectsInsideArea.Contains(foundObject)) {
            objectsInsideArea.Add(foundObject);
            ObjectType type = item.ObjectType;
            Types underlyingType = CheckItemType(type, item, enteringCabinet: true);
            if (underlyingType != Types.Null) {
                missingObjects[underlyingType]--;
            }
        }
    }

    private void UnfoldCloth() {

        if (folded) {
            return;
        }
        folded = true;

        if (sterileDrape == null) {
            Logger.Warning("Sterile drape not set in laminar cabinet, not performing animation.");
            return;
        }

        GameObject startState = sterileDrape.transform.GetChild(0).gameObject;
        GameObject endState = sterileDrape.transform.GetChild(1).gameObject;

        Destroy(startState);
        endState.SetActive(true);
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
                type = Types.BigSyringe;
            } else if (syringe.Container.Capacity == 1000) {
                type = Types.SmallSyringe;
            }
        } else if (itemType == ObjectType.Bottle) {
            type = Types.MedicineBottle;
        } else if (itemType == ObjectType.Needle) {
            type = Types.Needle;
        } else if (itemType == ObjectType.Luerlock) {
            type = Types.Luerlock;
        } else if (itemType == ObjectType.SyringeCapBag) {
            type = Types.SyringeCapBag;
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

            Logger.Print("Setting IsClean of caps inside laminar cabinet to " + capBag.IsClean);
            syringeCapFactory.GetComponent<GeneralItem>().IsClean = capBag.IsClean;
            bool capFactoryAlreadyEnabled = false;
            foreach (GameObject obj in objectsInsideArea) {
                GeneralItem item = obj.GetComponent<GeneralItem>();
                if (item.ObjectType == ObjectType.SyringeCap) {
                    item.IsClean = capBag.IsClean;
                    capFactoryAlreadyEnabled = true;
                }
            }

            if (!capFactoryAlreadyEnabled) {
                syringeCapFactory.SetActive(true);
            }

            capBag.DestroyInteractable();
        } else {
            Logger.Print("Syringe cap bag not inside cabinet anymore, won't destroy or set factory active");
        }
    }

    private void ReAddMissingObjects(Types itemType) {
        if (missingObjects.ContainsKey(itemType)) {
            switch (itemType) {
                case Types.Needle:
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.BigSyringe:
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.SmallSyringe:
                    if (missingObjects[itemType] < 6) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.Luerlock:
                    if (missingObjects[itemType] == 0) {
                        missingObjects[itemType]++;
                    }
                    break;
                case Types.MedicineBottle:
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
        String missing = "";
        foreach (KeyValuePair<Types, int> value in missingObjects) {
            if (value.Value > 0) {
                missing = missing + " " + value.Key + " " + value.Value + " kpl, \n";
            }
        }
        return missing;
    }
}