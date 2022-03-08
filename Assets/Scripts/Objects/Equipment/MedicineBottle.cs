using UnityEngine;
using UnityEngine.Assertions;

public class MedicineBottle : GeneralItem {

    #region fields
    public LiquidContainer Container { get; private set; }
    #endregion

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Attachable);
        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
    }
}
