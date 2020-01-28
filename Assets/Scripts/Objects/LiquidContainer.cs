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

        if (generalItem.ObjectType == ObjectType.Bottle) {
            syringe.State.On(InteractState.InBottle);
            syringe.hasBeenInBottle = true;
            Events.FireEvent(EventType.SyringeWithNeedleEntersBottle, CallbackData.Object(syringe));
            Events.FireEvent(EventType.Disinfect, CallbackData.Object(generalItem));
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

        if (generalItem.ObjectType == ObjectType.Bottle) {
            syringe.State.Off(InteractState.InBottle);
            syringe.BottleContainer = null;
        }
    }
}
