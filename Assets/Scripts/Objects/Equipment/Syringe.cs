using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region fields
    private LiquidContainer container;
    #endregion

    protected override void Start() {
        base.Start();
        container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(container);
        ObjectType = ObjectType.Syringe;

        Types.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid);
    }
}
