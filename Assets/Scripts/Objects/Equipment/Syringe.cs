using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region fields
    private LiquidContainer container;
    #endregion

    protected override void Start() {
        base.Start();
        container = GetComponent<LiquidContainer>();
        Assert.IsNotNull(container);
        ObjectType = ObjectType.Syringe;
    }
}
