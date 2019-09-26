using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region fields
    private LiquidContainer container;
    #endregion

    protected override void Start() {

        Logger.Print("SYRINGE START");

        base.Start();
        container = GetComponent<LiquidContainer>();
        Assert.IsNotNull(container);
        ObjectType = ObjectType.Syringe;

        Types.On(InteractableType.LuerlockAttachable);

        Debug.Log(Types.IsOn(InteractableType.Grabbable));
    }
}
