using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<Types, int> missingObjects;
    private bool itemPlaced = false;
    [SerializeField]
    private GameObject childCollider;

    [SerializeField]
    private Animator sterileDrape;

    private Dictionary<GeneralItem, bool> FirstEnterObjects;

    [SerializeField]
    [Tooltip("Used only in laminar cabinet. This factory will be set active when a SyringeCapBag has entered the laminar cabinet.")]
    private GameObject syringeCapFactory = null;
    [SerializeField]
    private GameObject syringeCapFactoryPos = null;

    private bool capFactoryEnabled = false;
    public bool CapFactoryEnabled => capFactoryEnabled;

    private TriggerInteractableContainer itemContainer;

    private Pipeline capBagEnterPipeline;
    private bool folded;
    #endregion

    // Start is called before the first frame update
    private void Start() {
        missingObjects = new Dictionary<Types, int>();
        missingObjects.Add(Types.Needle, 1);
        missingObjects.Add(Types.BigSyringe, 1);
        missingObjects.Add(Types.SmallSyringe, 6);
        missingObjects.Add(Types.Luerlock, 1);
        missingObjects.Add(Types.MedicineBottle, 1);
        missingObjects.Add(Types.SyringeCapBag, 1);

        itemContainer = childCollider.gameObject.AddComponent<TriggerInteractableContainer>();
        itemContainer.OnEnter = EnterCabinet;
        itemContainer.OnExit = ExitCabinet;

        FirstEnterObjects = new Dictionary<GeneralItem, bool>();

        if (syringeCapFactory != null) {
            syringeCapFactory.SetActive(false);
        }
    }

    private void EnterCabinet(Interactable other) {
        GeneralItem item = other as GeneralItem;
        if (item == null) {
            return;
        }

        if (FirstEnterObjects.ContainsKey(item)) {
            if (!FirstEnterObjects[item]) {
                UISystem.Instance.CreatePopup(-1, "Esineitä ei saa tuoda pois työskentelytilasta", MsgType.Mistake);
                G.Instance.Progress.Calculator.AddMistake("Esineitä ei saa tuoda pois työskentelytilasta");
                FirstEnterObjects[item] = true;
            }
        } else {
            FirstEnterObjects.Add(item, false);
        }

        if (item.Contamination == GeneralItem.ContaminateState.FloorContaminated && this.type == CabinetType.Laminar) {
            Logger.Print("Item was on floor: " + item.name);
            UISystem.Instance.CreatePopup(-1, "Lattialla olevia esineitä ei saa tuoda laminaarikaappiin", MsgType.Mistake);
            G.Instance.Progress.Calculator.AddMistake("Lattialla olevia esineitä ei saa tuoda laminaarikaappiin");

            // To force Contaminated state you need to set the state to Clean first. Look at the Contaminated property and fix it T. previous ryhmä
            item.Contamination = GeneralItem.ContaminateState.Clean;
            item.Contamination = GeneralItem.ContaminateState.Contaminated;
        }

        if (Time.timeSinceLevelLoad > 1) {
            UnfoldCloth();
        }

        if (!itemPlaced) {
            Events.FireEvent(EventType.ItemPlacedForReference, CallbackData.Object(this));
            itemPlaced = true;
        }
        if (this.type == CabinetType.Laminar) {
            Events.FireEvent(EventType.ItemPlacedInCabinet, CallbackData.Object(item));
        }

        ObjectType type = item.ObjectType;
        Types underlyingType = CheckItemType(type, item, enteringCabinet: true);
        if (underlyingType != Types.Null) {
            missingObjects[underlyingType]--;
        }
    }
    private void ExitCabinet(Interactable other) {

        GeneralItem item = other as GeneralItem;
        if (item == null) {
            return;
        }

        ObjectType type = item.ObjectType;
        Types underlyingType = CheckItemType(type, item, enteringCabinet: false);
        ReAddMissingObjects(underlyingType);
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
        sterileDrape.SetBool("ItemPlaced", true);
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

    public void DisableCapFactory() {
        syringeCapFactory.SetActive(false);
    }

    private void SyringeCapBagEnteredLaminarCabinet(GeneralItem capBag) {

        IEnumerator EnableCapFactory() {

            yield return new WaitForSeconds(2);

            if (!itemContainer.Contains(capBag)) {
                yield break;
            }

            GameObject meshCopy = new GameObject();

            foreach (Transform child in capBag.transform) {
                Vector3 lpos = child.localPosition;
                Vector3 lrot = child.localEulerAngles;

                Transform mesh = Instantiate(child.gameObject).transform;
                mesh.SetParent(meshCopy.transform);
                mesh.localPosition = lpos;
                mesh.localEulerAngles = lrot;
            }

            meshCopy.transform.position = capBag.transform.position;
            meshCopy.transform.rotation = capBag.transform.rotation;

            Vector3 startPos = capBag.transform.position;
            Quaternion startRot = capBag.transform.rotation;

            Vector3 targetPos = syringeCapFactoryPos.transform.position;
            Quaternion targetRot = syringeCapFactoryPos.transform.rotation;

            float time = 2.5f;
            float currentTime = 0;

            DestroyCapBagAndInitFactory(capBag);

            while (currentTime < time) {
                currentTime += Time.deltaTime;

                float progress = currentTime / time;

                meshCopy.transform.position = Vector3.Slerp(startPos, targetPos, progress);
                meshCopy.transform.rotation = Quaternion.Slerp(startRot, targetRot, progress);

                yield return null;
            }

            meshCopy.transform.position = targetPos;
            meshCopy.transform.rotation = targetRot;

            syringeCapFactory.SetActive(true);
            capFactoryEnabled = true;
        }

        StartCoroutine(EnableCapFactory());
    }

    private void DestroyCapBagAndInitFactory(GeneralItem capBag) {
        if (capBag != null && itemContainer.Contains(capBag)) {

            Logger.Print("Syringe cap bag still inside cabinet, destroying bag and setting factory active...");

            Logger.Print("Setting IsClean of caps inside laminar cabinet to " + capBag.IsClean);
            syringeCapFactory.GetComponent<GeneralItem>().Contamination = capBag.Contamination;
            foreach (Interactable obj in itemContainer.Objects) {
                GeneralItem item = obj as GeneralItem;
                if (item == null) {
                    continue;
                }
                if (item.ObjectType == ObjectType.SyringeCap) {
                    item.Contamination = capBag.Contamination;
                }
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
    public List<Interactable> GetContainedItems() {
        return itemContainer.Objects.ToList();
    }

    public string GetMissingItems() {
        string missing = "";
        foreach (KeyValuePair<Types, int> value in missingObjects) {
            if (value.Value > 0) {
                missing = missing + " " + value.Key + " " + value.Value + " kpl, \n";
            }
        }
        return missing;
    }
}