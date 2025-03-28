using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class AgarPlateBottom : GeneralItem {

    #region fields
    public LiquidContainer Container { get; private set; }
    #endregion
    public bool isOpen;
    public bool isVenting;
    public int spreadValue;
    public bool spreadingStatus;
    public UnityEvent<LiquidContainer> onSpreadingComplete;
    public UnityEvent onOpen;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Attachable, InteractableType.Interactable);
        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
    }

    void Update() {
        if (spreadValue >= 1000 && spreadingStatus == false) {
            Debug.Log("Spreading complete");
            spreadingStatus = true;
            onSpreadingComplete?.Invoke(Container);
        }
    }

    public void openAgarPlate() {
        onOpen.Invoke();
        isOpen = true;
    }

    public void closeAgarPlate() {
        isOpen = false;
    }
}
