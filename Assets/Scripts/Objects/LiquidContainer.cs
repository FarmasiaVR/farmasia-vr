using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LiquidContainer : MonoBehaviour {

    #region fields
    [SerializeField]
    private LiquidObject liquid;

    public delegate void AmountChange();
    public AmountChange OnAmountChange { get; set; }

    [SerializeField]
    private int amount;

    public LiquidType LiquidType;

    public GeneralItem GeneralItem;

    private TriggerInteractableContainer itemContainer;

    public bool Impure;

    public int Amount {
        get { return amount; }
    }

    public void SetAmountPercentage(float percentage) {
        int amount = (int)(percentage * Capacity);
        SetAmount(amount);
    }
    public void SetAmount(int value) {
        if (Capacity == 0) {
            amount = 0;
            liquid?.SetFillPercentage(0);
        } else {
            amount = Math.Max(Math.Min(value, Capacity), 0);
            // liquid is null when OnValidate is called twice before Awake
            // when playing in Editor Mode
            // See: https://forum.unity.com/threads/onvalidate-called-twice-when-pressing-play-in-the-editor.430250/
            float percentage = (float)amount / capacity;
            liquid?.SetFillPercentage(percentage);
        }
        OnAmountChange?.Invoke();
    }

    [SerializeField]
    private int capacity;
    public int Capacity {
        get { return capacity; }
        private set { capacity = Math.Max(value, 0); }
    }
    #endregion

    private void Awake() {
        Assert.IsNotNull(liquid);
    }


    private void Start() {
        GetComponent<MeshRenderer>().enabled = false;

        itemContainer = gameObject.AddComponent<TriggerInteractableContainer>();
        itemContainer.OnEnter = OnTrueEnter;
        itemContainer.OnExit = OnTrueExit;

        StartCoroutine(SearchInteractable());

        IEnumerator SearchInteractable() {

            yield return null;

            GeneralItem = (GeneralItem)Interactable.GetInteractable(transform);
        }
    }

    private void OnValidate() {
        Capacity = capacity;
        SetAmount(amount);
    }

    public int GetReceiveCapacity() {
        return Capacity - Amount;
    }

    public void TransferTo(LiquidContainer target, int amount) {
        if (target == null) {
            Logger.Error("Receiving LiquidContainer was null");
            return;
        }

        if (amount == 0) {
            return;
        }
        if (amount < 0) {
            target.TransferTo(this, -amount);
            return;
        }

        int receiveCapacity = target.GetReceiveCapacity();
        int canSend = Math.Min(Amount, amount);
        int toTransfer = Math.Min(canSend, receiveCapacity);

        if (toTransfer == 0) return;

        TransferLiquidType(target);

        SetAmount(Amount - toTransfer);
        target.SetAmount(target.Amount + toTransfer);

        FireBottleFillingEvent(target);
    }

    private void TransferLiquidType(LiquidContainer target) {
        if (target.LiquidType == LiquidType || target.LiquidType == LiquidType.None) {
            target.LiquidType = LiquidType;
        } else { // Case: the target has held or holds different liquid
            if (target.Amount == 0) {
                target.LiquidType = LiquidType;
                target.Impure = true;
            } else {
                target.Impure = true;
            }
        }
    }

    private void FireBottleFillingEvent(LiquidContainer target) {
        if (target.GeneralItem is MedicineBottle || target.GeneralItem is PumpFilter) {
            Events.FireEvent(EventType.TransferLiquidToBottle, CallbackData.Object(target));
        }
    }

    /// <summary>
    /// Tries to find a LiquidContainer component from the Transform component,
    /// or from its child object named "Liquid"
    /// </summary>
    /// <param name="t">The Transform of the GameObject to search</param>
    /// <returns>The LiquidContainer found or null</returns>
    public static LiquidContainer FindLiquidContainer(Transform t) {

        LiquidContainer c = t.GetComponent<LiquidContainer>();

        if (c != null) {
            return c;
        }

        return t.Find("Liquid")?.GetComponent<LiquidContainer>();
    }

    private void OnTrueEnter(Interactable enteringInteractable) {
        Needle needle = enteringInteractable as Needle;
        Pipette pipette = enteringInteractable as Pipette;
        if (needle!=null && needle.Connector.HasAttachedObject) {
            OnSyringeEnter(needle);
        }
        
        if (pipette!=null) {
            OnPipetteEnter(pipette);
        }

        
    }

    private void OnSyringeEnter(Needle needle) {
        Syringe syringe = needle.Connector.AttachedInteractable as Syringe;
        if (syringe == null) {
            return;
        }

        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine) {
            syringe.State.On(InteractState.InBottle);
            syringe.hasBeenInBottle = true;

            if (!GeneralItem.IsClean) {
                needle.Contamination = GeneralItem.ContaminateState.Contaminated;
            }

            if (G.Instance.Scene is MedicinePreparationScene) {
                if ((G.Instance.Scene as MedicinePreparationScene).NeedleUsed) {
                    TaskBase.CreateGeneralMistake("L��kett� yritettiin ottaa uudestaan");
                }
            }

            Events.FireEvent(EventType.SyringeWithNeedleEntersBottle, CallbackData.Object(syringe));
        }

        syringe.BottleContainer = this;
    }

    private void OnPipetteEnter(Pipette pipette) {
        if (GeneralItem is MedicineBottle) {
            pipette.State.On(InteractState.InBottle);
            pipette.hasBeenInBottle = true;

            if (!GeneralItem.IsClean) {
                pipette.Contamination = GeneralItem.ContaminateState.Contaminated;
            }

            Events.FireEvent(EventType.SyringeWithNeedleEntersBottle, CallbackData.Object(pipette));
        }

        pipette.BottleContainer = this;
    }

    private void OnTrueExit(Interactable enteringInteractable) {
        Needle needle = enteringInteractable as Needle;
        if (needle != null) {
            OnNeedleExit(needle);
        }

        Pipette pipette = enteringInteractable as Pipette;
        if (pipette != null) {
            OnPipetteExit(pipette);
        }
        
    }

    private void OnNeedleExit(Needle needle) {
        Syringe syringe = needle.Connector.AttachedInteractable as Syringe;
        if (syringe == null) {
            return;
        }

        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine) {
            syringe.State.Off(InteractState.InBottle);
            syringe.BottleContainer = null;
        }
    }

    private void OnPipetteExit(Pipette pipette) {
        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine) {
            pipette.State.Off(InteractState.InBottle);
            pipette.BottleContainer = null;
        }
    }
}
