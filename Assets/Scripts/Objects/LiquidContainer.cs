using System;
using UnityEngine;
using UnityEngine.Assertions;

public class LiquidContainer : MonoBehaviour {

    #region fields
    [SerializeField]
    private LiquidObject liquid;

    [SerializeField]
    private int amount;

    public int Amount {
        get { return amount; }
        set {
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
        }
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
    }

    private void OnValidate() {
        Capacity = capacity;
        Amount = amount;
    }

    public int GetReceiveCapacity() {
        return Capacity - Amount;
    }

    public void TransferTo(LiquidContainer target, int amount) {
        if (amount < 0) {
            throw new ArgumentException("value must be non-negative", "amount");
        }
        int canSend = Math.Min(Amount, amount);
        int toTransfer = Math.Min(canSend, target.GetReceiveCapacity());

        Amount -= toTransfer;
        target.Amount += toTransfer;
    }

    public static LiquidContainer FindLiquidContainer(Transform t) {

        LiquidContainer c = t.GetComponent<LiquidContainer>();

        if (c != null) {
            return c;
        }

        return t.Find("Liquid")?.GetComponent<LiquidContainer>();
    }
}
