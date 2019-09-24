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
            liquid.SetFillPercentage(((float) amount) / capacity);
        }
    }

    [SerializeField]
    private int capacity;
    public int Capacity {
        get { return capacity; }
        set { capacity = value; }
    }
    #endregion

    private void Start() {
        liquid = GetComponentInChildren<LiquidObject>();
        Assert.IsNotNull(liquid);
    }

    private void OnValidate() {
        Amount = amount;
    }
}
