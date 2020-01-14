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

    private ContainerItem cItem;

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

        StartCoroutine(SearchInteractable());

        IEnumerator SearchInteractable() {

            yield return null;

            Interactable interactable = Interactable.GetInteractable(transform);

            //Logger.Print("interactable found: " + interactable.name);

            GeneralItem gItem = (GeneralItem)interactable;

            if (gItem == null) {
                throw new Exception("Liquid container attached to non GeneralItem object");
            }

            cItem = new ContainerItem(this, gItem);
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

    private void OnTriggerEnter(Collider c) {
        cItem?.TriggerEnter(c);
    }
 
    private void OnTriggerExit(Collider c) {
        cItem?.TriggerExit(c);
    }
}
