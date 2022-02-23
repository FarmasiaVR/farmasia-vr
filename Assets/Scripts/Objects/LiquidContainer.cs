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

    [SerializeField]
    private LiquidType liquidType;

    private GeneralItem generalItem;

    private TriggerInteractableContainer itemContainer;

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

            generalItem = (GeneralItem)Interactable.GetInteractable(transform);
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

        ApplyLiquidType(target);

        int receiveCapacity = target.GetReceiveCapacity();
        int canSend = Math.Min(Amount, amount);
        int toTransfer = Math.Min(canSend, receiveCapacity);

        Logger.Print("Transferring " + toTransfer + " in total");

        SetAmount(Amount - toTransfer);
        target.SetAmount(target.Amount + toTransfer);
    }

    private void ApplyLiquidType(LiquidContainer target) {
        if (target.liquidType == LiquidType.None) {
            target.liquidType = liquidType;
        } else {
            target.liquidType = LiquidType.Mixed;
        }
        Logger.Print("Receiving LiquidContainer's type set to " + target.liquidType);
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

        if (generalItem.ObjectType == ObjectType.Bottle || generalItem.ObjectType == ObjectType.Medicine) {
            syringe.State.On(InteractState.InBottle);
            syringe.hasBeenInBottle = true;

            if (!generalItem.IsClean) {
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
        if (generalItem is MedicineBottle) {
            pipette.State.On(InteractState.InBottle);
            pipette.hasBeenInBottle = true;

            if (!generalItem.IsClean) {
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

        if (generalItem.ObjectType == ObjectType.Bottle || generalItem.ObjectType == ObjectType.Medicine) {
            syringe.State.Off(InteractState.InBottle);
            syringe.BottleContainer = null;
        }
    }

    private void OnPipetteExit(Pipette pipette) {
        if (generalItem.ObjectType == ObjectType.Bottle || generalItem.ObjectType == ObjectType.Medicine) {
            pipette.State.Off(InteractState.InBottle);
            pipette.BottleContainer = null;
        }
    }
}
