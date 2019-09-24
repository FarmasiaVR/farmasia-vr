using UnityEngine;
using UnityEngine.Assertions;

public class MedicineBottle : GeneralItem {

    #region fields
    private LiquidContainer container;
    #endregion

    protected override void Start() {
        base.Start();
        container = GetComponent<LiquidContainer>();
        Assert.IsNotNull(container);
        ObjectType = ObjectType.Bottle;
    }
}
