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

        if (amount == 0) return;
        if (amount < 0) {
            target.TransferTo(this, -amount);
            return;
        }

        int receiveCapacity = target.GetReceiveCapacity();
        int canSend = Math.Min(Amount, amount);
        int toTransfer = Math.Min(canSend, receiveCapacity);

        SetAmount(Amount - toTransfer);
        target.SetAmount(target.Amount + toTransfer);
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
        if (needle == null || !needle.Connector.HasAttachedObject) {
            return;
        }

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
                    TaskBase.CreateGeneralMistake("L‰‰kett‰ yritettiin ottaa uudestaan");
                }
            }

            Events.FireEvent(EventType.SyringeWithNeedleEntersBottle, CallbackData.Object(syringe));
        }

        syringe.BottleContainer = this;
    }

    private void OnTrueExit(Interactable enteringInteractable) {

        Needle needle = enteringInteractable as Needle;
        if (needle == null) {
            return;
        }

        Syringe syringe = needle.Connector.AttachedInteractable as Syringe;
        if (syringe == null) {
            return;
        }

        if (generalItem.ObjectType == ObjectType.Bottle || generalItem.ObjectType == ObjectType.Medicine) {
            syringe.State.Off(InteractState.InBottle);
            syringe.BottleContainer = null;
        }
    }
}
