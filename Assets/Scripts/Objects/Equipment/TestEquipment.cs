using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TestEquipment : GeneralItem {

    [SerializeField]
    private GameObject targetObject;
    private LiquidContainer other;
    private LiquidContainer container;

    protected override void Start() {
        base.Start();
        container = GetComponent<LiquidContainer>();
        Assert.IsNotNull(container);
        other = targetObject.GetComponent<LiquidContainer>();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            container.Transfer(other, 10);
        }
    }
}
