using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region fields
    public LiquidContainer Container { get; private set; }

    [SerializeField]
    private float defaultPosition, maxPosition;

    [SerializeField]
    private Transform handle;
    #endregion

    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
        ObjectType = ObjectType.Syringe;

        Types.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid);

        Container.OnAmountChange += SetSyringeHandlePosition;
    }

    public void SetSyringeHandlePosition() {
        Vector3 pos = handle.localPosition;
        pos.y = SyringePos();
        handle.localPosition = pos;
    }

    private float SyringePos() {
        float factor = 1.0f * Container.Amount / Container.Capacity;

        return factor * (maxPosition - defaultPosition);
    }
}
