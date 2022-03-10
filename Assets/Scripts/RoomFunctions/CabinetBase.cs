using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CabinetRequiredItems {
    public static Dictionary<ObjectType, int> ForMedicinePreparation() {
        var items = new Dictionary<ObjectType, int>();
        items.Add(ObjectType.Needle, 1);
        items.Add(ObjectType.BigSyringe, 1);
        items.Add(ObjectType.Syringe, 6);
        items.Add(ObjectType.Luerlock, 1);
        items.Add(ObjectType.Medicine, 1);
        items.Add(ObjectType.SyringeCapBag, 1);

        return items;
    }

    public static Dictionary<ObjectType, int> ForMembraneFilteration() {
        var items = new Dictionary<ObjectType, int>();
        items.Add(ObjectType.Scalpel, 1);

        return items;
    }

}

public class CabinetBase : MonoBehaviour {

    // Muistoja:
    /*public enum Types {
        Null,
        Needle,
        BigSyringe,
        SmallSyringe,
        Luerlock,
        MedicineBottle,
        SyringeCapBag
    }*/

    #region Fields
    public enum CabinetType { PassThrough, Laminar }
    [SerializeField]
    public CabinetType type;
    private Dictionary<Type, int> missingObjects;
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
    private bool hasCapBag;
    private GameObject capBagMeshCopy;
    private GeneralItem trueCapBag;
    #endregion

    // Start is called before the first frame update
    private void Start() {
        switch (G.Instance.CurrentSceneType)
        {
            case (SceneTypes.MedicinePreparation): missingObjects = GetMedicinePreparationRequiredItems(); break;
            case (SceneTypes.MembraneFilteration): missingObjects = GetMembraneFilterationRequiredItems(); break;
            default: missingObjects = GetMedicinePreparationRequiredItems(); break;
        }

        itemContainer = childCollider.gameObject.AddComponent<TriggerInteractableContainer>();
        itemContainer.OnEnter = EnterCabinet;
        itemContainer.OnExit = ExitCabinet;

        FirstEnterObjects = new Dictionary<GeneralItem, bool>();

        if (syringeCapFactory != null) {
            syringeCapFactory.SetActive(false);
        }
    }

    private Dictionary<Type, int> GetMedicinePreparationRequiredItems() {
        var items = new Dictionary<Type, int>();
        items.Add(typeof(Needle), 1);
        items.Add(typeof(LuerlockAdapter), 1);
        items.Add(typeof(Bottle), 1);
        items.Add(typeof(SmallSyringe), 6);
        items.Add(typeof(Syringe), 1);
        items.Add(typeof(SyringeCap), 1);
        return items;
    }

    private Dictionary<Type, int> GetMembraneFilterationRequiredItems() {
        var items = new Dictionary<Type, int>();
        items.Add(typeof(Pipette), 1);
        return items;
    }

    private void EnterCabinet(Interactable other) {
        GeneralItem item = other as GeneralItem;
        if (item == null) {
            return;
        }

        if (this.type == CabinetType.Laminar) {
            if (FirstEnterObjects.ContainsKey(item)) {
                if (!FirstEnterObjects[item]) {
                    Task.CreateGeneralMistake("Esineitä ei saa tuoda pois työskentelytilasta");
                    FirstEnterObjects[item] = true;
                }
            } else {
                FirstEnterObjects.Add(item, false);
            }
        }

        if (item.Contamination == GeneralItem.ContaminateState.FloorContaminated && this.type == CabinetType.Laminar) {
            Logger.Print("Item was on floor: " + item.name);
            Task.CreateGeneralMistake("Lattialla olevia esineitä ei saa tuoda laminaarikaappiin");

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

        if (item is SyringeCapBag) {
            if (this.type == CabinetType.Laminar) {
                SyringeCapBagEnteredLaminarCabinet(item);
            }
        }

        if (missingObjects.ContainsKey(item.GetType()))
            missingObjects[item.GetType()]--;
    }
    private void ExitCabinet(Interactable other) {

        GeneralItem item = other as GeneralItem;
        if (item == null) {
            return;
        }

        ReAddMissingObjects(other);
    }

    private void UnfoldCloth() {
        if (folded) {
            return;
        }
        folded = true;
        if (sterileDrape == null) {
            return;
        }
        sterileDrape.SetBool("ItemPlaced", true);
    }

    private void SyringeCapBagEnteredLaminarCabinet(GeneralItem capBag) {
        if (hasCapBag) return;

        trueCapBag = capBag;
        hasCapBag = true;
        StartCoroutine(EnableCapFactory());

        IEnumerator EnableCapFactory() {

            yield return new WaitForSeconds(2);

            if (!itemContainer.Contains(trueCapBag)) {
                yield break;
            }

            capBagMeshCopy = new GameObject();

            foreach (Transform child in trueCapBag.transform) {
                Vector3 lpos = child.localPosition;
                Vector3 lrot = child.localEulerAngles;

                Transform mesh = Instantiate(child.gameObject).transform;
                mesh.SetParent(capBagMeshCopy.transform);
                mesh.localPosition = lpos;
                mesh.localEulerAngles = lrot;
            }

            capBagMeshCopy.transform.position = trueCapBag.transform.position;
            capBagMeshCopy.transform.rotation = trueCapBag.transform.rotation;

            Vector3 startPos = trueCapBag.transform.position;
            Quaternion startRot = trueCapBag.transform.rotation;

            Vector3 targetPos = syringeCapFactoryPos.transform.position;
            Quaternion targetRot = syringeCapFactoryPos.transform.rotation;

            float time = 2.5f;
            float currentTime = 0;

            DestroyCapBagAndInitFactory();

            while (currentTime < time) {
                currentTime += Time.deltaTime;

                float progress = currentTime / time;

                capBagMeshCopy.transform.position = Vector3.Slerp(startPos, targetPos, progress);
                capBagMeshCopy.transform.rotation = Quaternion.Slerp(startRot, targetRot, progress);

                yield return null;
            }

            capBagMeshCopy.transform.position = targetPos;
            capBagMeshCopy.transform.rotation = targetRot;

            syringeCapFactory.SetActive(true);
            capFactoryEnabled = true;
        }
    }

    private void DestroyCapBagAndInitFactory() {
        if (trueCapBag != null && itemContainer.Contains(trueCapBag)) {

            Logger.Print("Syringe cap bag still inside cabinet, destroying bag and setting factory active...");

            Logger.Print("Setting IsClean of caps inside laminar cabinet to " + trueCapBag.IsClean);
            syringeCapFactory.GetComponent<GeneralItem>().Contamination = trueCapBag.Contamination;
            foreach (Interactable obj in itemContainer.Objects) {
                GeneralItem item = obj as GeneralItem;
                if (item == null) {
                    continue;
                }
                if (item.ObjectType == ObjectType.SyringeCap) {
                    item.Contamination = trueCapBag.Contamination;
                }
            }

            //capBag.DestroyInteractable();
            DisableTrueCapBag();
        } else {
            Logger.Print("Syringe cap bag not inside cabinet anymore, won't destroy or set factory active");
        }
    }

    private void DisableTrueCapBag() {
        trueCapBag.gameObject.SetActive(false);
    }

    private void EnableTrueCapBag() {
        trueCapBag.gameObject.SetActive(true);
    }

    private void ReAddMissingObjects(Interactable other) {
        if (!missingObjects.ContainsKey(other.GetType())) return;

        if (other is SmallSyringe)
            if (missingObjects[typeof(SmallSyringe)] < 6) missingObjects[typeof(SmallSyringe)]++;
        else
            if (missingObjects[other.GetType()] == 0) missingObjects[other.GetType()]++;
    }

    /// <summary>
    /// Returns list presentation of contained gameobjects.
    /// </summary>
    public List<Interactable> GetContainedItems() {
        return itemContainer.Objects.ToList();
    }

    public string GetMissingItems() {
        string missing = "";
        foreach (KeyValuePair<Type, int> value in missingObjects) {
            if (value.Value > 0) {
                missing = string.Format("{0} {1} {2} kpl, \n", missing, value.Key.ToString(), value.Value.ToString());
            }
        }
        return missing;
    }
}