using System;
using UnityEngine;
using UnityEngine.Assertions;

public class LiquidContainer : MonoBehaviour {

    #region fields
    private LiquidObject liquid;

    [SerializeField]
    private int amount;
    public int Amount {
        get { return amount; }
        set {
            amount = Math.Max(Math.Min(value, Capacity), 0);
            // liquid is null when OnValidate is called twice before Awake
            // when playing in Editor Mode
            // See: https://forum.unity.com/threads/onvalidate-called-twice-when-pressing-play-in-the-editor.430250/
            liquid?.SetFillPercentage(((float) amount) / capacity);
        }
    }

    [SerializeField]
    private int capacity;
    public int Capacity {
        get { return capacity; }
        set { capacity = value; }
    }
    #endregion

    private void Awake() {
        liquid = GetComponentInChildren<LiquidObject>();
        Assert.IsNotNull(liquid);
    }

    private void OnValidate() {
        Amount = amount;
    }

    public int Transfer(int value) {
        int oldAmount = Amount;
        Amount += value;
        return oldAmount - Amount;
    }

    public void Transfer(LiquidContainer container, int value) {
        int transferAmount = Math.Max(Amount - Capacity, Math.Min(Amount, value));
        Transfer(container.Transfer(value));
    }
}
